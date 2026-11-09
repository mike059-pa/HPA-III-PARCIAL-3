using Parte3_LoginBCrypt.Negocio;

namespace Parte3_LoginBCrypt;

public class FrmLogin : Form
{
    private readonly TextBox txtUsuario;
    private readonly TextBox txtClave;
    private readonly Label lblMensaje;
    private readonly Button btnEntrar;

    public FrmLogin()
    {
        Estilo.AplicarFormulario(this, "Login BCrypt - HPA III Parcial #3", 380, 340);

        Controls.Add(Estilo.Etiqueta("Iniciar sesión", 30, 20, Estilo.Titulo));
        Controls.Add(Estilo.Etiqueta("Contraseñas protegidas con BCrypt", 32, 58));

        Controls.Add(Estilo.Etiqueta("Usuario", 30, 95));
        txtUsuario = Estilo.Caja(30, 117, 320);
        Controls.Add(txtUsuario);

        Controls.Add(Estilo.Etiqueta("Contraseña", 30, 152));
        txtClave = Estilo.Caja(30, 174, 320, clave: true);
        Controls.Add(txtClave);

        lblMensaje = new Label { Location = new Point(30, 208), Size = new Size(320, 22), ForeColor = Estilo.Error };
        Controls.Add(lblMensaje);

        btnEntrar = Estilo.Boton("Entrar", 30, 236, 320);
        btnEntrar.Click += (_, _) => Entrar();
        Controls.Add(btnEntrar);

        var btnRegistro = Estilo.Boton("Crear cuenta", 30, 280, 320, primario: false);
        btnRegistro.Click += (_, _) =>
        {
            using var frm = new FrmRegistro();
            if (frm.ShowDialog(this) == DialogResult.OK)
            {
                txtUsuario.Text = frm.UsuarioCreado;
                txtClave.Clear();
                txtClave.Focus();
                MostrarMensaje("Cuenta creada. Inicie sesión.", Estilo.Exito);
            }
        };
        Controls.Add(btnRegistro);

        AcceptButton = btnEntrar;
    }

    private void Entrar()
    {
        if (string.IsNullOrWhiteSpace(txtUsuario.Text) || txtClave.Text.Length == 0)
        {
            MostrarMensaje("Ingrese usuario y contraseña.", Estilo.Error);
            return;
        }

        try
        {
            UseWaitCursor = true;
            btnEntrar.Enabled = false;
            var usuario = AuthService.IniciarSesion(txtUsuario.Text, txtClave.Text);

            if (usuario is null)
            {
                MostrarMensaje("Usuario o contraseña incorrectos.", Estilo.Error);
                txtClave.SelectAll();
                txtClave.Focus();
                return;
            }

            // Acceso exitoso: se abre la bienvenida mostrando el nombre del usuario.
            MostrarMensaje("", Estilo.Exito);
            txtClave.Clear();
            UseWaitCursor = false;
            Hide();
            using (var frm = new FrmBienvenida(usuario))
                frm.ShowDialog();
            Show();
            txtClave.Focus();
        }
        catch (Exception ex)
        {
            MessageBox.Show(this, "No se pudo conectar con la base de datos:\n\n" + ex.Message,
                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            UseWaitCursor = false;
            btnEntrar.Enabled = true;
        }
    }

    private void MostrarMensaje(string texto, Color color)
    {
        lblMensaje.ForeColor = color;
        lblMensaje.Text = texto;
    }
}
