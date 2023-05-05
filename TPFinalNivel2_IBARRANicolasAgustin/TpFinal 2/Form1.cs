using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Negocio;
using dominio;
using Datos;

namespace TpFinal_2
{
    public partial class Form1 : Form
    {
        private List<Articulos> listaArticulos;
        public Form1()
        {
            InitializeComponent();
        }
        public Form1(Articulos articulos)
        {
            InitializeComponent();
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            VentanaAgregar ventana = new VentanaAgregar();
            ventana.ShowDialog();
            cargar();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            cbxFiltroAvanzado.Items.Add("Marca");
            cbxFiltroAvanzado.Items.Add("Categoría");
            cbxFiltroAvanzado.Items.Add("Precio");
            cbxFiltroAvanzado.Items.Add("Descripción");
            cargar();

        }
        private void cargar()
        {
            ArticulosNegocio negocio = new ArticulosNegocio();
            try
            {
                listaArticulos = negocio.listar();
                dgvCatalogo.DataSource = listaArticulos;
                ocultarColumnas();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }
        private void ocultarColumnas()
        {
            try
            {
                dgvCatalogo.Columns["ImagenUrl"].Visible = false;
                dgvCatalogo.Columns["Id"].Visible = false;
                dgvCatalogo.Columns[1].Visible = false;
                dgvCatalogo.Columns[3].Visible = false;
               
                pbxCatalogo.Load(listaArticulos[0].ImagenUrl);

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void dgvCatalogo_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvCatalogo.CurrentRow != null)
            {
                Articulos seleccionado = (Articulos)dgvCatalogo.CurrentRow.DataBoundItem;
                CargarImagen(seleccionado.ImagenUrl);
            }
            
        }
        private void CargarImagen(string imagen)
        {
            try
            {
                pbxCatalogo.Load(imagen);
            }
            catch (Exception ex)
            {
                pbxCatalogo.Load("https://agropro.ag/wp-content/uploads/2023/04/placeholder-289.png");

            }



        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvCatalogo.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Por favor seleccione un artículo de la lista antes de continuar.", "Artículo no seleccionado", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                Articulos seleccionado;
                seleccionado = (Articulos)dgvCatalogo.CurrentRow.DataBoundItem;
                VentanaAgregar modificar = new VentanaAgregar(seleccionado);
                modificar.ShowDialog();
                cargar();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
           
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
           AccesoDatos negocio = new AccesoDatos();
            Articulos seleccionado = new Articulos();
            try
            {
                DialogResult resultado = MessageBox.Show("¿De verdad quieres eliminarlo?", "eliminar", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                
                
                if (resultado == DialogResult.Yes)
                {
                    seleccionado = (Articulos)dgvCatalogo.CurrentRow.DataBoundItem;
                    negocio.eliminar(seleccionado.Id);
                    cargar();
                }
                



            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }

        private void btnFiltro_Click(object sender, EventArgs e)
        {
           
        }

        private void tbxFiltro_TextChanged(object sender, EventArgs e)
        {
            List<Articulos> listaFiltrada;
            string filtro = tbxFiltro.Text;

            if (filtro.Length >= 2 )
            {
                listaFiltrada = listaArticulos.FindAll(a => a.Nombre.ToLower().Contains(filtro.ToLower()) || a.Descripcion.ToLower().Contains(filtro.ToLower()));
            }
            else
            {
                listaFiltrada = listaArticulos;
            }


            dgvCatalogo.DataSource = null;
            dgvCatalogo.DataSource = listaFiltrada;
            ocultarColumnas();
        }

        private void cbxFiltroAvanzado_SelectedIndexChanged(object sender, EventArgs e)
        {
            string opcion = cbxFiltroAvanzado.SelectedItem.ToString();
            if (opcion == "Precio")
            {
                cbxCriterio.Items.Clear();
                cbxCriterio.Items.Add("Mayor a: ");
                cbxCriterio.Items.Add("Menor a: ");
                cbxCriterio.Items.Add("Igual a: ");
            }
            else
            {
                cbxCriterio.Items.Clear();
                cbxCriterio.Items.Add("Comienza con: ");
                cbxCriterio.Items.Add("Termina con: ");
                cbxCriterio.Items.Add("Contiene: ");
            }
            if (cbxFiltroAvanzado.SelectedItem.ToString() == "Precio")
            {
                txtFiltroAvanzado.KeyPress += new KeyPressEventHandler(OnlyNumbers_KeyPress);
            }
            else
            {
                txtFiltroAvanzado.KeyPress -= OnlyNumbers_KeyPress;
            }
        }
        private void OnlyNumbers_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
        }

        private void btnFiltroAvanzado_Click(object sender, EventArgs e)
        {
            ArticulosNegocio articulosNegocio = new ArticulosNegocio(); 
            try
            {
                string campo = cbxFiltroAvanzado.SelectedItem?.ToString();
                string criterio = cbxCriterio.SelectedItem?.ToString();
                string filtro = txtFiltroAvanzado.Text;

                bool isCampoEmpty = string.IsNullOrEmpty(campo);
                bool isCriterio = string.IsNullOrEmpty(criterio);
                bool isFiltroEmpty = string.IsNullOrEmpty(filtro);  

                if (isCampoEmpty || isCriterio || isFiltroEmpty)
                {
                    MessageBox.Show("Por favor seleccione un valor para cada campo de filtro.", "Complete los campos",MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                    if (isCampoEmpty)
                    {
                        lblFiltroAvanzado.ForeColor = Color.Red; 
                    }
                    if (isCriterio)
                    {
                        lblCriteriofiltro.ForeColor = Color.Red;
                    }
                    if (isFiltroEmpty)
                    {
                        lblBuscarFiltro.ForeColor = Color.Red;
                    }
                   

                    return;

                }

                lblFiltroAvanzado.ForeColor = Color.Black;
                lblCriteriofiltro.ForeColor = Color.Black;
                lblBuscarFiltro.ForeColor = Color.Black;





                dgvCatalogo.DataSource = articulosNegocio.filtrar(campo, criterio, filtro);

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }


        }

        private void btnRefrescar_Click(object sender, EventArgs e)
        {
            dgvCatalogo.DataSource = listaArticulos;
        }

        private void btnVerDetalle_Click(object sender, EventArgs e)
        {
            if (dgvCatalogo.SelectedRows.Count == 0)
            {
                MessageBox.Show("Por favor seleccione un artículo de la lista antes de continuar.", "Artículo no seleccionado", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            Articulos seleccionado;
            seleccionado = (Articulos)dgvCatalogo.CurrentRow.DataBoundItem;
            txtVerDetalles.Text = seleccionado.ToString();
        }
    }
}
