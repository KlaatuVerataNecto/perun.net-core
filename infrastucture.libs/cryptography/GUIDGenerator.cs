using System;

namespace infrastucture.libs.cryptography
{
    public static class GUIDGenerator
    {
        public static string ToAlphaNumerical()
        {
            Guid hashid = Guid.NewGuid();
            var hash = hashid.ToString().Replace("-", "");
            return hash;
        }
    }
}
