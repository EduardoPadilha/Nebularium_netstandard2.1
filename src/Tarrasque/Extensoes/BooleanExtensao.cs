namespace Nebularium.Tarrasque.Extensoes
{
    public static class BooleanExtensao
    {
        public static string Descricao(this bool s)
        {
            return s.Descricao("Sim", "Não");
        }
        public static string Descricao(this bool s, string verdadeiro, string falso)
        {
            return s ? verdadeiro : falso;
        }
    }
}
