using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using dominio;
using Negocio;
using Datos;
using System.IO;
using System.Configuration;

namespace TpFinal_2
{
    public partial class VentanaAgregar : Form
    {
        private Articulos articulos = null; 
        private OpenFileDialog archivo = null;
        public VentanaAgregar()
        {
            InitializeComponent();
        }
        public VentanaAgregar(Articulos articulos)
        {
            InitializeComponent();
            this.articulos = articulos;
            Text = "Modificar";
            lblTituloAgregar.Text = "Modificar Articulo";
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
           
            AccesoDatos negocio = new AccesoDatos();
            try
            {
                if(articulos == null)
                    articulos = new Articulos();    

                articulos.CodigoArticulo = txtCodigoArticulo.Text;
                articulos.Nombre = txtNombre.Text;   
                articulos.Descripcion = txtDescripcion.Text;
                articulos.ImagenUrl = txtImagen.Text;
                //articulos.Precio = decimal.Parse(txtPrecio.Text);
                if (string.IsNullOrEmpty(txtPrecio.Text))
                {
                    MessageBox.Show("El campo de precio está vacío. Por favor, ingrese un valor válido.", "Error de entrada", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                else
                {
                    articulos.Precio = decimal.Parse(txtPrecio.Text);
                }
                articulos.Categoria = (Categoria)cbxCategoria.SelectedItem;
                articulos.Marca = (Marca)cbxMarca.SelectedItem;
                articulos.ImagenUrl = txtImagen.Text;

                string codigo = txtCodigoArticulo.Text;
                string nombre = txtNombre.Text;
                string marca = cbxMarca.SelectedItem?.ToString();
                string categoria = cbxCategoria.SelectedItem?.ToString();
                string precio = txtPrecio.Text;

                bool codigoArt = string.IsNullOrEmpty(codigo);
                bool Nombre = string.IsNullOrEmpty(nombre);
                bool Marca = string.IsNullOrEmpty(marca);
                bool Categoria = string.IsNullOrEmpty(categoria);
                bool Precio = string.IsNullOrEmpty(precio);

                if (codigoArt || Nombre || Marca || Categoria || Precio)
                {
                    validaciones();
                    return;
                }
                else
                {
                    if (articulos.Id != 0)
                    {
                        negocio.Modificar(articulos);
                        MessageBox.Show("Modificado exitosamente");

                    }
                    else
                    {

                        negocio.Agregar(articulos);
                        MessageBox.Show("Agregado exitosamente");


                    }
                }

               

                if (archivo != null && !(txtImagen.Text.ToLower().Contains("http")))
                {
                    string destino = ConfigurationManager.AppSettings["CatalogoApp"] + archivo.SafeFileName;
                    if (!File.Exists(destino))
                    {
                        File.Copy(archivo.FileName, destino);
                    }
                }

                

                Close();

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }
        private void validaciones()
        {
            string codigo = txtCodigoArticulo.Text;
            string nombre = txtNombre.Text;
            string marca = cbxMarca.SelectedItem?.ToString();
            string categoria = cbxCategoria.SelectedItem?.ToString();
            string precio = txtPrecio.Text;

            bool codigoArt = string.IsNullOrEmpty(codigo);
            bool Nombre = string.IsNullOrEmpty(nombre);
            bool Marca = string.IsNullOrEmpty(marca);
            bool Categoria = string.IsNullOrEmpty(categoria);
            bool Precio = string.IsNullOrEmpty(precio);

            if (codigoArt || Nombre || Marca || Categoria || Precio)
            {
                MessageBox.Show("Por favor complete los campos obligatorios.", "Complete los campos", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                if (codigoArt)
                {
                    lblasteriscoCAr.ForeColor = Color.Red;
                }
                if (Nombre)
                {
                    lblasteriscoNombre.ForeColor = Color.Red;
                }
                if (Marca)
                {
                    lblasteriscoMarca.ForeColor = Color.Red;
                }
                if (Categoria)
                {
                    lblasteriscoCategoria.ForeColor = Color.Red;
                }
                if (Precio)
                {
                    lblasteriscoPrecio.ForeColor = Color.Red;
                }
                return;
            }

            lblasteriscoCAr.ForeColor = Color.Black;
            lblasteriscoNombre.ForeColor = Color.Black;
            lblasteriscoMarca.ForeColor = Color.Black;
            lblasteriscoCategoria.ForeColor = Color.Black;
            lblasteriscoPrecio.ForeColor = Color.Black;
        }

        private void VentanaAgregar_Load(object sender, EventArgs e)
        {
            MarcaNegocio negocioMarca = new MarcaNegocio();
            CategoriaNegocio negocioCategoria = new CategoriaNegocio();

            try
            {
                cbxMarca.DataSource = negocioMarca.listar();
                cbxMarca.ValueMember = "Id";
                cbxMarca.DisplayMember = "Descripcion";
                cbxCategoria.DataSource = negocioCategoria.listar();
                cbxCategoria.ValueMember = "Id";
                cbxCategoria.DisplayMember = "Descripcion";
                
                if (articulos != null)
                {
                    txtCodigoArticulo.Text = articulos.CodigoArticulo;
                    txtDescripcion.Text = articulos.Descripcion;
                    txtNombre.Text = articulos.Nombre;
                    txtPrecio.Text = articulos.Precio.ToString();
                    txtImagen.Text = articulos.ImagenUrl;
                    CargarImagen(articulos.ImagenUrl);
                    cbxMarca.SelectedValue = articulos.Marca.Id;   
                    cbxCategoria.SelectedValue = articulos.Categoria.Id;

                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }

        private void txtImagen_Leave(object sender, EventArgs e)
        {
            CargarImagen(txtImagen.Text);
        }
        private void CargarImagen(string imagen)
        {
            try
            {
                pbxVentana.Load(imagen);
            }
            catch (Exception ex)
            {
                pbxVentana.Load("https://agropro.ag/wp-content/uploads/2023/04/placeholder-289.png");

            }



        }

        private void btnCargarImagen_Click(object sender, EventArgs e)
        {
            archivo = new OpenFileDialog();
            archivo.Filter = "jpg|*.jpg;|png|*.png";
            if(archivo.ShowDialog() == DialogResult.OK)
            {
                txtImagen.Text= archivo.FileName;
                CargarImagen(archivo.FileName);
            }
            
           

        }

        private void txtPrecio_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }
    }
}
