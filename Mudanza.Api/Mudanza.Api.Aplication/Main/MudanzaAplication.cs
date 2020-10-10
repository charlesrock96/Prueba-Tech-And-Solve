using Microsoft.AspNetCore.Http;
using Mudanza.Api.Aplication.Dto;
using Mudanza.Api.Aplication.Interface;
using Mudanza.Api.Domain.Entity;
using Mudanza.Api.Infraestructure.Interface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mudanza.Api.Aplication.Main
{
    public class MudanzaAplication : IMudanzaAplication
    {
        readonly ILogRespository _logRepository;

        public MudanzaAplication(ILogRespository logRepository)
        {
            _logRepository = logRepository;
        }

        public async Task<ResultadoDto> EjecutarProcesoMudanzaAsync(MudanzaDto mudanza)
        {
            String[] arregloDatos;
            EntradasDto estrutura;
            ResultadoDto resultado;
            bool resultadoEntity;

            resultado = await ValidarDatosAsync(mudanza);

            if (!resultado.Error)
            {
                //cargamos el archivo a ser procesado
                arregloDatos = await CargarArchivoAsync(mudanza.Archivo);

                //leemos el arreglo de datos para convertirlo en una estructura
                estrutura = LeerArregloDatos(arregloDatos);

                //Procesamos los datos para calcular los viajes por dia
                resultado.Salida = ProcesarDatos(estrutura);

                //se almacena la traza
                resultadoEntity = await _logRepository.InsertAsync(new TblLog
                {
                    Cedula = mudanza.Cedula,
                    Fecha = DateTime.Now,
                    Traza = resultado.Salida
                });

                if (!resultadoEntity) return new ResultadoDto { Error = true, ErrorMensaje = "Error al guardar la traza." };
            }

            return resultado;
        }
        
        public string ProcesarDatos(EntradasDto estrutura)
        {
            String traza = "";
            int cargaMinima = 50;
            var salidas = new List<String>();
            try
            {
                //Recorremos la estructura para calcular los viajes
                foreach (var item in estrutura.LstElementosPorDia)
                {
                    int totalCarga = 0;
                    int totalViajes = 0;
                    var pesos = item.LstElementosACargar;
                    PesoDto pesoMax = new PesoDto(0);

                    //mientras hayan pesos que procesar
                    while (pesos.Count > 0)
                    {
                        if (totalCarga == 0)
                        {
                            //calculamos la carga maxima y luego la removemos de la estructura
                            pesoMax = pesos.Max(); 
                            pesos.Remove(pesoMax);
                            totalCarga += pesoMax.Peso;
                        }
                        else
                        {
                            //calculamos la carga minima y luego la removemos de la estructura
                            var pesoMin = pesos.Min();
                            pesos.Remove(pesoMin);
                            totalCarga += pesoMax.Peso;
                        }

                        // si el total de la carga supera las 50 libras reiniciamos el proceso
                        if (totalCarga >= cargaMinima)
                        {
                            totalViajes++;
                            totalCarga = 0;
                        }
                    }

                    //Añadimos la salida calculada
                    salidas.Add($"Case #{ salidas.Count() + 1 }: { totalViajes }");
                }

                //construimos la traza
                foreach (var item in salidas)
                    traza += $"{ item }{ "\n" }";

                return traza;
            }
            catch (Exception)
            {
                return "Error al procesar los datos.";
            }
            
        }

        public async Task<string[]> CargarArchivoAsync(IFormFile archivo)
        {
            String datosLeidos;

            //leemos el archivo para convertirlo en un vector
            using (var lector = new StreamReader(archivo.OpenReadStream()))
            {
                datosLeidos = await lector.ReadToEndAsync();                 
            }

            return datosLeidos.Split('\n');
        }

        public EntradasDto LeerArregloDatos(String[] datos)
        {
            int numeroElementosPorDia = 0;
            var ElementosPorDia = new ElementosDto();
            var estrutura = new EntradasDto { LstElementosPorDia = new List<ElementosDto>()};

            //Recorremos el arreglo de datos para cargarlo en una estructura
            for (int i = 0; i < datos.Length; i++)
            {
                //leemos el valor correspondeiente a la posicion
                int valor = Int32.Parse(datos[i]);

                if (i == 0) // el primer elemento hace referencia al numero de dias
                {
                    estrutura.NumeroDias = valor;
                }
                else if (numeroElementosPorDia == 0) //asignamos el numero de elementos por dia
                {
                    ElementosPorDia = new ElementosDto { NumeroElementosPorDia = valor };
                    numeroElementosPorDia = valor;  //guargamos el valor para que en el siguiente ciclo no se lea
                }
                else
                {
                    //Se recorre el arreglo en base an numero  de elementos por dia y se cargan los pesos
                    //de cada elemento correspondiente a la lista de pesos
                    var lstPesos = new List<PesoDto>();

                    for (int j = i; j < i + numeroElementosPorDia; j++)
                    {
                        valor = Int32.Parse(datos[j]);

                        lstPesos.Add(new PesoDto(valor));
                    }

                    if (lstPesos.Count > 0)
                    {
                        ElementosPorDia.LstElementosACargar = lstPesos; // se cargan los pesos al elemento
                        estrutura.LstElementosPorDia.Add(ElementosPorDia); // se carga el elemnto a la estructura

                    }

                    i += numeroElementosPorDia - 1; //actulizamos el indice en base a los elementos procesados
                    numeroElementosPorDia = 0; //reiniciamos el valor para que en el siguiente ciclo se lea
                }
            }

            return estrutura;
        }

        public async Task<ResultadoDto> ValidarDatosAsync(MudanzaDto mudanza)
        {
            string mensaje;

            if (String.IsNullOrEmpty(mudanza.Cedula))
            {
                mensaje = "El campo Cédula es obligatorio.";
            }
            else if (mudanza.Archivo == null)
            {
                mensaje = "El campo Archivo es obligatorio.";
            }
            else if(mudanza.Archivo.Length == 0)
            {
                mensaje = "El archivo esta vacio.";
            }
            else if (mudanza.Archivo.Length > 1000000)
            {
                mensaje = "El archivo es demasiado grande.";
            }
            else if(!mudanza.Archivo.ContentType.Contains("text/plain"))
            {
                mensaje = "Se esperaba un archivo .txt";
            }
            else
            {
                mensaje = await ValidarDatosArchivoAsync(mudanza.Archivo);
            }

            return new ResultadoDto {
                Error = !String.IsNullOrEmpty(mensaje),
                ErrorMensaje = mensaje
            };
        }

        public async Task<string> ValidarDatosArchivoAsync(IFormFile archivo)
        {
            try
            {
                String[] arregloDatos;
                int numeroElementosPorDia = 0;
                var ElementosPorDia = new ElementosDto();
                var estrutura = new EntradasDto { LstElementosPorDia = new List<ElementosDto>() };

                String datosLeidos;

                //leemos el archivo
                using (var lector = new StreamReader(archivo.OpenReadStream()))
                {
                    datosLeidos = await lector.ReadToEndAsync();
                }

                //convertimos los datos en un arreglo de datos
                arregloDatos = datosLeidos.Split('\n');

                if (arregloDatos.Length < 3)
                    return "No se pudo procesar el archivo.";

                //verificamos que todos los datos sean numericos
                foreach (var item in arregloDatos)
                {
                    bool success = Int32.TryParse(item, out int numero);

                    if (!success || numero < 1 || numero > 500)
                        return "Todos los elementos del archivo deben ser numeros enteros entre 1 y 500";
                }


                //Recorremos el arreglo de datos para cargarlo en una estructura
                for (int i = 0; i < arregloDatos.Length; i++)
                {
                    //leemos el valor correspondeiente a la posicion
                    int valor = Int32.Parse(arregloDatos[i]);

                    if (i == 0) // el primer elemento hace referencia al numero de dias
                    {
                        if (valor < 1 || valor > 500)
                            return "El numero de dias a trabajar debe estar entre 1 y 500.";

                        estrutura.NumeroDias = valor;
                    }
                    else if (numeroElementosPorDia == 0) //asignamos el numero de elementos por dia
                    {
                        if (valor < 1 || valor > 100)
                            return "El numero de elementos a cargar por dia debe estar entre 1 y 100.";

                        ElementosPorDia.NumeroElementosPorDia = valor;
                        numeroElementosPorDia = valor;  //guargamos el valor para que en el siguiente ciclo no se lea
                    }
                    else
                    {
                        //Se recorre el arreglo en base an numero  de elementos por dia y se cargan los pesos
                        //de cada elemento correspondiente a la lista de pesos
                        var lstPesos = new List<PesoDto>();

                        for (int j = i; j < i + numeroElementosPorDia; j++)
                        {
                            valor = Int32.Parse(arregloDatos[j]);

                            if (valor < 1 || valor > 100)
                                return "El peso de los elementos a cargar debe estar entre 1 y 100.";

                            lstPesos.Add(new PesoDto(valor));
                        }

                        if (lstPesos.Count > 0)
                        {
                            ElementosPorDia.LstElementosACargar = lstPesos; // se cargan los pesos al elemento
                            estrutura.LstElementosPorDia.Add(ElementosPorDia); // se carga el elemnto a la estructura

                        }

                        i += numeroElementosPorDia - 1; //actulizamos el indice en base a los elementos procesados
                        numeroElementosPorDia = 0; //reiniciamos el valor para que en el siguiente ciclo se lea
                    }
                }

                //validamos que la estructura sea coherente
                if(estrutura.NumeroDias != estrutura.LstElementosPorDia.Count)
                    return "Los datos en el archivo no tienen coherencia.";

                foreach (var item in estrutura.LstElementosPorDia)
                {
                    if (item.NumeroElementosPorDia != item.LstElementosACargar.Count)
                        return "Los datos en el archivo no tienen coherencia.";
                }

                return String.Empty;
            }
            catch (Exception)
            {
                return "La cantidad de elementos en el archivo no es la correcta.";
            }
        }
    }
}
