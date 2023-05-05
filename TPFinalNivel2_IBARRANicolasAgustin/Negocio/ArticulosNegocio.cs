using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dominio;
using Datos;
using System.Net;
using System.Data.SqlTypes;
using System.Collections;

namespace Negocio
{
    public class ArticulosNegocio
    {
        public List<Articulos> filtrar(string campo, string criterio, string filtro)
        {
            List<Articulos> articulos = new List<Articulos>();
            AccesoDatos datos = new AccesoDatos();
            
            try
            {
                string consulta = "select codigo, nombre, A.descripcion Descripcion,c.Descripcion Categoria,m.Descripcion Marca, imagenUrl, precio, A.IdCategoria, A.IdMarca, A.Id from ARTICULOS A, CATEGORIAS C, MARCAS M where A.IdCategoria = C.Id and A.IdMarca = m.Id and ";

                switch (campo)
                {
                    case "Precio":
                        switch (criterio)
                        {
                            case "Mayor a: ":
                                consulta += "Precio > " + filtro;
                                break;

                            case "Menor a: ":
                                consulta += "Precio < " + filtro;
                                break;

                            default:
                                consulta += "Precio = " + filtro;
                                break;
                        }
                        break;

                    case "Descripción":
                        switch (criterio)
                        {
                            case "Comienza con: ":
                                consulta += "A.Descripcion like '" + filtro + "%'";
                                break;
                            case "Termina con: ":
                                consulta += "A.Descripcion like '%" + filtro + "'";
                                break;
                            default:
                                consulta += "A.Descripcion '%" + filtro + "%'";
                                break;
                        }
                        break;

                    case "Marca":
                        switch (criterio)
                        {
                            case "Comienza con: ":
                                consulta += "m.Descripcion like '" + filtro + "%'";
                                break;
                            case "Termina con: ":
                                consulta += "m.Descripcion like '%" + filtro + "'";
                                break;
                            default:
                                consulta += "m.Descripcion '%" + filtro + "%'";
                                break;
                        }
                        break;

                    case "Categoría":
                        switch (criterio)
                        {
                            case "Comienza con: ":
                                consulta += "c.Descripcion like '" + filtro + "%'"; 
                                break;
                            case "Termina con: ":
                                consulta += "c.Descripcion like '%" + filtro +"'";
                                break;
                            default:
                                consulta += "c.Descripcion '%" + filtro +"%'";
                                break;
                        }
                        break;
                        



                }

                datos.setearConsulta(consulta);
                datos.ejecutarLectura();
                while (datos.Lector.Read())
                {
                    Articulos aux = new Articulos();
                    aux.Id = (int)datos.Lector["Id"];

                    if (!(datos.Lector.IsDBNull(datos.Lector.GetOrdinal("codigo"))))
                        aux.CodigoArticulo = (string)datos.Lector["codigo"];

                    if (!(datos.Lector.IsDBNull(datos.Lector.GetOrdinal("nombre"))))
                        aux.Nombre = (string)datos.Lector["nombre"];

                    if (!(datos.Lector.IsDBNull(datos.Lector.GetOrdinal("Descripcion"))))
                        aux.Descripcion = (string)datos.Lector["Descripcion"];

                    aux.Categoria = new Categoria();
                    aux.Categoria.Id = (int)datos.Lector["IdCategoria"];
                    aux.Categoria.Descripcion = (string)datos.Lector["Categoria"];
                    aux.Marca = new Marca();
                    aux.Marca.Id = (int)datos.Lector["IdMarca"];
                    aux.Marca.Descripcion = (string)datos.Lector["Marca"];

                    if (!(datos.Lector.IsDBNull(datos.Lector.GetOrdinal("imagenUrl"))))
                        aux.ImagenUrl = (string)datos.Lector["imagenUrl"];

                    if (!(datos.Lector.IsDBNull(datos.Lector.GetOrdinal("precio"))))
                        aux.Precio = (decimal)datos.Lector["precio"];

                    articulos.Add(aux);
                }
                
                return articulos;

                

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public List<Articulos> listar()
        {
			List<Articulos> lista = new List<Articulos>();
			AccesoDatos datos = new AccesoDatos();

			try
			{
				datos.setearConsulta("select codigo, nombre, A.descripcion Descripcion,c.Descripcion Categoria,m.Descripcion Marca, imagenUrl, precio, A.IdCategoria, A.IdMarca, A.Id from ARTICULOS A, CATEGORIAS C, MARCAS M where A.IdCategoria = C.Id and A.IdMarca = m.Id");
				datos.ejecutarLectura();
				
                while (datos.Lector.Read())
				{
                    Articulos aux = new Articulos();
                    aux.Id = (int)datos.Lector["Id"];

                    if (!(datos.Lector.IsDBNull(datos.Lector.GetOrdinal("codigo"))))
                        aux.CodigoArticulo = (string)datos.Lector["codigo"];

                    if (!(datos.Lector.IsDBNull(datos.Lector.GetOrdinal("nombre"))))
                        aux.Nombre = (string)datos.Lector["nombre"];

                    if (!(datos.Lector.IsDBNull(datos.Lector.GetOrdinal("Descripcion"))))
                        aux.Descripcion = (string)datos.Lector["Descripcion"];
                   
                    aux.Categoria = new Categoria();
                    aux.Categoria.Id = (int)datos.Lector["IdCategoria"];
                    aux.Categoria.Descripcion = (string)datos.Lector["Categoria"];
                    aux.Marca = new Marca();
                    aux.Marca.Id = (int)datos.Lector["IdMarca"];
                    aux.Marca.Descripcion = (string)datos.Lector["Marca"];

                    if (!(datos.Lector.IsDBNull(datos.Lector.GetOrdinal("imagenUrl")))) 
                        aux.ImagenUrl = (string)datos.Lector["imagenUrl"];

                    if (!(datos.Lector.IsDBNull(datos.Lector.GetOrdinal("precio"))))
                        aux.Precio = (decimal)datos.Lector["precio"];
                    
                    lista.Add(aux);
                }

				
				
				return lista;
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
       

    }

}
