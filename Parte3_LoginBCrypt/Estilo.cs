namespace Parte3_LoginBCrypt;

/// <summary>Colores y fuentes comunes para los formularios.</summary>
internal static class Estilo
{
    public static readonly Color Fondo = Color.FromArgb(245, 247, 250);
    public static readonly Color Primario = Color.FromArgb(37, 99, 235);
    public static readonly Color Texto = Color.FromArgb(31, 41, 55);
    public static readonly Color Exito = Color.FromArgb(22, 163, 74);
    public static readonly Color Error = Color.FromArgb(220, 38, 38);

    public static readonly Font Titulo = new("Segoe UI", 16f, FontStyle.Bold);
    public static readonly Font Normal = new("Segoe UI", 10f);

    public static void AplicarFormulario(Form f, string titulo, int ancho, int alto)
    {
        f.Text = titulo;
        f.ClientSize = new Size(ancho, alto);
        f.BackColor = Fondo;
        f.Font = Normal;
        f.ForeColor = Texto;
        f.StartPosition = FormStartPosition.CenterScreen;
        f.FormBorderStyle = FormBorderStyle.FixedDialog;
        f.MaximizeBox = false;
    }

    public static Label Etiqueta(string texto, int x, int y, Font? fuente = null) =>
        new() { Text = texto, Location = new Point(x, y), AutoSize = true, Font = fuente ?? Normal };

    public static TextBox Caja(int x, int y, int ancho, bool clave = false) =>
        new() { Location = new Point(x, y), Width = ancho, UseSystemPasswordChar = clave };

    public static Button Boton(string texto, int x, int y, int ancho, bool primario = true)
    {
        var b = new Button
        {
            Text = texto, Location = new Point(x, y), Size = new Size(ancho, 36),
            FlatStyle = FlatStyle.Flat, Cursor = Cursors.Hand,
            BackColor = primario ? Primario : Color.White,
            ForeColor = primario ? Color.White : Primario
        };
        b.FlatAppearance.BorderColor = Primario;
        return b;
    }
}
