using System;
using System.Collections.Generic;
using System.Linq;
using GymWebApp.Models;

namespace GymWebApp.Repositories
{
    public class MemberRepository
    {
        private static readonly List<Member> Store = new List<Member>();

        public bool AddMember(Member m, out string error)
        {
            error = null;
            if (m == null) { error = "Bad member data."; return false; }

            var emailKey = NormalizeEmail(m.Email);
            var phoneKey = NormalizePhone(m.Phone);

            if (Store.Any(x => NormalizeEmail(x.Email) == emailKey))
            {
                error = "Email is already registered.";
                return false;
            }

            if (Store.Any(x => NormalizePhone(x.Phone) == phoneKey))
            {
                error = "Phone number is already registered.";
                return false;
            }

            Store.Add(m);
            return true;
        }

        public List<Member> GetAll() => Store.ToList();

        public Member FindByEmail(string email)
        {
            var key = NormalizeEmail(email);
            return Store.FirstOrDefault(m => NormalizeEmail(m.Email) == key);
        }

        public Member FindByPhone(string phone)
        {
            var key = NormalizePhone(phone);
            return Store.FirstOrDefault(m => NormalizePhone(m.Phone) == key);
        }

        public int Count => Store.Count;

        private static string NormalizeEmail(string email) =>
            (email ?? string.Empty).Trim().ToLowerInvariant();

        private static string NormalizePhone(string phone)
        {
            if (string.IsNullOrWhiteSpace(phone)) return string.Empty;
            var digits = new string(phone.Where(char.IsDigit).ToArray());
            return digits;
        }
    }
}