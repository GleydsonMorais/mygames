using System;
using System.Collections.Generic;
using System.Text;

namespace MyGames.Data.Constants
{
    public class AccountConstants
    {
        /// <summary>
        /// admin
        /// </summary>
        public static readonly string AdminUser = "admin";

        /// <sumary>
        /// Administrador
        /// </sumary>>
        public static readonly string Administrador = "Administrador";

        /// <sumary>
        /// Amigo
        /// </sumary>>
        public static readonly string Amigo = "Amigo";

        /// <sumary>
        /// 1
        /// </sumary>>
        public static readonly int AdministradorDB = 1;

        /// <sumary>
        /// 2
        /// </sumary>>
        public static readonly int AmigoDB = 2;

        /// <sumary>
        /// Recebe 1 ou 2 e retorna Administrador ou Amigo
        /// </sumary>>
        public static string GetPerfilUsuario(int perfilDB)
        {
            return perfilDB == AdministradorDB ? Administrador : Amigo;
        }

        /// <summary>
        /// Ativo
        /// </summary>
        public static readonly string StatusAtivo = "Ativo";
        /// <summary>
        /// Inativo
        /// </summary>
        public static readonly string StatusInativo = "Inativo";

        /// <summary>
        /// Recebe true ou false e retorna Ativo ou Inativo
        /// </summary>
        public static string GetStatusUsuario(bool statusDB)
        {
            return statusDB ? StatusAtivo : StatusInativo;
        }

        /// <summary>
        /// Recebe Ativo ou Inativo e retorna true ou false
        /// </summary>
        public static bool GetStatusDbUsuario(string status)
        {
            return status == "Ativo" ? true : false;
        }
    }
}
