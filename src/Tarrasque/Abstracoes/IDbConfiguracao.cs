﻿namespace Nebularium.Tarrasque.Abstracoes
{
    public interface IDbConfiguracao : IConfiguracao
    {
        string StringConexao { get; set; }
        string NomeBancoDados { get; set; }
        bool LogLigado { get; set; }
    }
}
