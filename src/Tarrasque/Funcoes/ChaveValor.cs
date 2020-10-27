namespace Nebularium.Tarrasque.Funcoes
{
    public class ChaveValor
    {
        public string Descricao { get; set; }
        public string Valor { get; set; }
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }
            return Equals((ChaveValor)obj);
        }
        public bool Equals(ChaveValor obj)
        {
            return obj.Valor == Valor;
        }
        public override int GetHashCode()
        {
            return Valor.GetHashCode();
        }
    }
}
