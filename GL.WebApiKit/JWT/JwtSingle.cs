namespace GL.WebApiKit.JWT
{
    public class JwtSingle
    {
        private JwtSingle() { }

        public static JwtSingle Instance { get; } = new JwtSingle();

        public JwtHelper JWT { get; } = new JwtHelper();
    }
}
