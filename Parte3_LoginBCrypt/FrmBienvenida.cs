using Parte3_LoginBCrypt.Datos;
using Parte3_LoginBCrypt.Entidades;

namespace Parte3_LoginBCrypt;

public class FrmBienvenida : Form
{
    public FrmBienvenida(Usuario usuario)
    {
        Estilo.AplicarFormulario(this, "Bienvenido", 640, 430);

        // Acceso exitoso: se indica el nombre del usuario.
        Controls.Add(new Label
        {
            Text = $"¡Bienvenido, {usuario.NombreCompleto}!",
            Location = new Point(25, 20), AutoSize = true,
            Font = Estilo.Titulo, ForeColor = Estilo.Exito
        });
        Controls.Add(Estilo.Etiqueta($"Usuario: {usuario.NombreUsuario}", 27, 60));
        Controls.Add(Estilo.Etiqueta($"Hash BCrypt almacenado: {usuario.PasswordHash}", 27, 84));

        Controls.Add(Estilo.Etiqueta("Últimos intentos de inicio de sesión de este usuario:", 27, 120));
        var grid = new DataGridView
        {
            Location = new Point(25, 145), Size = new Size(590, 220),
            ReadOnly = true, AllowUserToAddRows = false, AllowUserToDeleteRows = false,
            RowHeadersVisible = false, BackgroundColor = Color.White,
            AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
            SelectionMode = DataGridViewSelectionMode.FullRowSelect
        };
        grid.DataBindingComplete += (_, _) =>
        {
            foreach (DataGridViewRow fila in grid.Rows)
                fila.DefaultCellStyle.ForeColor =
                    fila.Cells["Resultado"].Value?.ToString() == "Exitoso" ? Estilo.Exito : Estilo.Error;
            if (grid.Columns["Fecha"] is { } col) col.DefaultCellStyle.Format = "dd/MM/yyyy HH:mm:ss";
        };
        Controls.Add(grid);

        try
        {
            grid.DataSource = UsuarioDatos.ConsultarIntentos(usuario.NombreUsuario, 20);
        }
        catch (Exception ex)
        {
            Controls.Add(Estilo.Etiqueta("No se pudo cargar la bitácora: " + ex.Message, 27, 370));
        }

        var btnCerrar = Estilo.Boton("Cerrar sesión", 465, 378, 150);
        btnCerrar.Click += (_, _) => Close();
        Controls.Add(btnCerrar);
    }
}
