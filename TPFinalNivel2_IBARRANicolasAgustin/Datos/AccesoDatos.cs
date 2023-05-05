using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using dominio;
using System.Runtime.Remoting.Messaging;

namespace Datos
{
    public class AccesoDatos
    {
        private SqlConnection conexion;
        private SqlCommand comando;
        private SqlDataReader lector;

        public SqlDataReader Lector
        {
            get { return lector; }
        }

        public AccesoDatos()
        {
            conexion = new SqlConnection("server= .\\SQLEXPRESS; database= CATALOGO_DB; integrated security= true;");
            comando = new SqlCommand();
        }
        
        public void setearConsulta(string consulta)
        {
            comando.CommandType = System.Data.CommandType.Text;
            comando.CommandText = consulta;
        }
        public void ejecutarLectura()
        {
            comando.Connection = conexion;
            try
            {
                conexion.Open();
                lector = comando.ExecuteReader();
            }
            catch (Exception ex)
            {

                throw ex;
            }  
        }
        public void Modificar(Articulos arti)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta("update ARTICULOS set codigo = @Codigo,Nombre = @Nombre, Descripcion = @Desc, IdMarca = @IdMarca, IdCategoria = @IdCategoria, ImagenUrl = @Img, Precio = @Precio where Id = @Id");
                datos.setearParametros("@Codigo",arti.CodigoArticulo);
                datos.setearParametros("@Desc", arti.Descripcion);
                datos.setearParametros("@IdMarca", arti.Marca.Id);
                datos.setearParametros("@IdCategoria", arti.Categoria.Id);
                datos.setearParametros("@Img", arti.ImagenUrl);
                datos.setearParametros("@Precio", arti.Precio);
                datos.setearParametros("@Id", arti.Id);
                datos.setearParametros("@Nombre", arti.Nombre);
                datos.ejecutarAccion();
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }
        public void Agregar(Articulos articulos)
        {   AccesoDatos datos = new AccesoDatos();  
            try
            {
                datos.setearConsulta("insert into ARTICULOS (Codigo, Nombre, Descripcion, ImagenUrl, Precio, IdMarca, IdCategoria) values ('" + articulos.CodigoArticulo + "', '" + articulos.Nombre + "', '" + articulos.Descripcion + "', '" + articulos.ImagenUrl + "', '" + articulos.Precio + "', @IdMarca, @IdCategoria)");
                datos.setearParametros("@IdMarca", articulos.Marca.Id);
                datos.setearParametros("@IdCategoria", articulos.Categoria.Id);
                datos.ejecutarAccion();

            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally { datos.cerrarConexion();}
        }
        public void cerrarConexion()
        {
            if(lector != null)
                lector.Close();
            conexion.Close();
        }
        public void ejecutarAccion()
        {
            comando.Connection = conexion;
            try
            {
                conexion.Open() ;
                comando.ExecuteNonQuery();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public void setearParametros(string nombre, object valor)
        {
            comando.Parameters.AddWithValue(nombre, valor);
        }
        public void eliminar(int Id)
        {
            try
            {
                AccesoDatos datos = new AccesoDatos();
                datos.setearConsulta("delete from ARTICULOS where Id = @Id");
                datos.setearParametros("@Id", Id);
                datos.ejecutarAccion();

            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
    }
}
