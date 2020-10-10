import React, { useState } from 'react';
import axios from 'axios';
import swal from 'sweetalert';

const Formulario = () => { 

    const baseUrl="https://localhost:44330/api/mudanza";
    let fileInput;

    const [datos, setDatos] = useState({
        cedula : '',
        archivo : null
    })

    const handleInputChange =(event) => {
        setDatos({
           ...datos,
           cedula : event.target.value
        })
    }

    const handleInputChangeFile =(event) => {
        setDatos({
           ...datos,
           archivo : event.target.files[0]
        })
    }

    const mostrarAlert = (mensaje, tipo)=>{
        swal({
            title: "",
            text: mensaje === "" ? "Datos procesados correctamente" : mensaje,
            icon: tipo ? "error" : "success",
            button: "Aceptar"
        });
    }

    const handleSubmit = (event) => {
        event.preventDefault();
        let formData = new FormData();
        formData.append('archivo', datos.archivo)
        formData.append('cedula', datos.cedula)    
    
        axios.post(baseUrl, formData, {
            headers: {
                'Content-Type': 'multipart/form-data'
            }
        }).then(resp => {
            fileInput.value = "";

            setDatos({
                ...datos,
                cedula : ''
            });

            mostrarAlert(resp.data.errorMensaje, resp.data.error);

            if(!resp.data.error){
                descargarArchivo(resp.data.salida, 'lazy_loading_output.txt');
            }          
            
        }).catch(error=>{
            mostrarAlert("Al parecer los servicios no estan disponibles", true);;
        })

    }

    const descargarArchivo = (contenido, nombreArchivo) => {
        let blob = new Blob([contenido], { type: 'text/plain' }); 
        let link = document.createElement('a');
        link.href = window.URL.createObjectURL(blob);
        link.download = nombreArchivo;
        link.click(); 
    };
    
    return (
        <>
            <h1 className="text-center">Mudanzas</h1>
            <form onSubmit={handleSubmit}>
                <div className="form-group">
                    <label>Cédula</label>
                    <input 
                        type="text" 
                        className="form-control" 
                        id="cedula" 
                        name="cedula" 
                        placeholder="Cédula"
                        onChange={handleInputChange}
                        value={datos.cedula}
                        required />
                </div>
                <div className="form-group">
                    <label>Archivo</label>
                    <input 
                        type="file" 
                        className="form-control" 
                        id="archivo" 
                        name="archivo" 
                        placeholder="Archivo" 
                        accept=".txt"
                        onChange={handleInputChangeFile}
                        ref={ref => fileInput = ref}
                        required />
                </div>
                <button type="Submit" className="btn btn-primary" >Procesar</button>
            </form>
        </>
    );
}

export default Formulario;