using System;
using System.Collections.Generic;
using System.Text;

namespace MyGames.Data.Constants
{
    public class GamesConstants
    {
        /// <sumary>
        /// Tabuleiro
        /// </sumary>>
        public static readonly string Tabuleiro = "Tabuleiro";

        /// <sumary>
        /// Cartas
        /// </sumary>>
        public static readonly string Cartas = "Cartas";

        /// <sumary>
        /// PS4
        /// </sumary>>
        public static readonly string PS4 = "PS4";

        /// <sumary>
        /// XBox
        /// </sumary>>
        public static readonly string XBox = "XBox";

        /// <sumary>
        /// 1
        /// </sumary>>
        public static readonly int TabuleiroDB = 1;

        /// <sumary>
        /// 2
        /// </sumary>>
        public static readonly int CartasDB = 2;

        /// <sumary>
        /// 3
        /// </sumary>>
        public static readonly int PS4DB = 3;

        /// <sumary>
        /// 4
        /// </sumary>>
        public static readonly int XBoxDB = 4;

        /// <summary>
        /// Recebe true ou false e retorna Sim ou Não
        /// </summary>
        public static string GetStatusDevolucao(bool statusDB)
        {
            return statusDB ? "Sim" : "Não";
        }

        /// <summary>
        /// Recebe Sim ou Não e retorna true ou false
        /// </summary>
        public static bool GetStatusDbDevolucao(string status)
        {
            return status == "Sim" ? true : false;
        }
    }
}
