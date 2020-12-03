Web Service ASMX c# encargado de conectar el aplicativo web cliente a la base de datos.

Inserta facturas xlsx, en bd utilizando OPENROWSET(Microsoft.ACE.OLEDB.12.0), para aquello primero mueve el archivo xlsx a servidor de dase de datos y luego inserta su contenido en tablas temporales.
Valida  datos factuas xlsx con procedimientos almacenados, (nulos,tipo datos, largo, fechas,rut,etc).
Elimina tabla temporales.


Autoria: Ivan Sobarzo

PD: se elimino webconfig porque tenia informacion sensibles de acceso a base de datos.
