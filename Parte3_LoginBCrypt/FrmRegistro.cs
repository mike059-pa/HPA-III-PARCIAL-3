using Parte3_LoginBCrypt.Negocio;

namespace Parte3_LoginBCrypt;

public class FrmRegistro : Form
{
    private readonly TextBox txtNombre;
    private readonly TextBox txtUsuario;
    private readonly TextBox txtClave;
    private readonly TextBox txtConfirmar;

    public string UsuarioCreado { get; private set; } = "";

    public FrmRegistro()
    {
        Estilo.AplicarFormulario(this, "Crear cuenta", 380, 400);

        Controls.Add(Estilo.Etiqueta("Crear cuenta", 30, 20, Estilo.Titulo));

        Controls.Add(Estilo.Etiqueta("Nombre completo", 30, 70));
        txtNombre = Estilo.Caja(30, 92, 320);
        Controls.Add(txtNombre);

        Controls.Add(Estilo.Etiqueta("Usuario", 30, 127));
        txtUsuario = Estilo.Caja(30, 149, 320);
        Controls.Add(txtUsuario);

        Controls.Add(Estilo.Etiqueta("Contraseña (mín. 6 caracteres)", 30, 184));
        txtClave = Estilo.Caja(30, 206, 320, clave: true);
        Controls.Add(txtClave);

        Controls.Add(Estilo.Etiqueta("Confirmar contraseña", 30, 241));
        txtConfirmar = Estilo.Caja(30, 263, 320, clave: true);
        Controls.Add(txtConfirmar);

        var btnGuardar = Estilo.Boton("Registrar", 30, 305, 320);
        btnGuardar.Click += (_, _) => Guardar();
        Controls.Add(btnGuardar);

        var btnCancelar = Estilo.Boton("Cancelar", 30, 349, 320, primario: false);
        btnCancelar.DialogResult = DialogResult.Cancel;
        Controls.Add(btnCancelar);

        AcceptButton = btnGuardar;
        CancelButton = btnCancelar;
    }

    private void Guardar()
    {
        if (txtClave.Text != txtConfirmar.Text)
        {
            MessageBox.Show(this, "Las contraseñas no coinciden.", "Validación",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        try
        {
            // El hash BCrypt se genera aquí y es lo único que se guarda en la BD.
            AuthService.RegistrarUsuario(txtUsuario.Text, txtNombre.Text, txtClave.Text);
            UsuarioCreado = txtUsuario.Text.Trim();
            DialogResult = DialogResult.OK;
        }
        catch (ArgumentException ex)
        {
            MessageBox.Show(this, ex.Message, "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
        catch (Exception ex)
        {
            MessageBox.Show(this, "Error al registrar:\n\n" + ex.Message, "Error",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
