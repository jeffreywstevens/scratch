// --------------------------------------------------------------------------------------------------------------------
// <copyright file="VaultSecretClientConfig.cs" company="Orion Fusion Inc">
//   Copyright (c) 2022 Orion Fusion Inc. All Rights Reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace mykvscretclient
{
    public class VaultSecretClientConfig : IVaultSecretClientConfig
    {
        public VaultSecretClientConfig(string tenantId, string keyVaultUri, string clientId, string certificateThumbprint)
        {
            this.TenantId = tenantId;
            this.KeyVaultUri = keyVaultUri;
            this.ClientId = clientId;
            this.CertificateThumbprint = certificateThumbprint;
        }

        public string TenantId { get; }
        public string KeyVaultUri { get; }
        public string ClientId { get; }
        public string CertificateThumbprint { get; }
    }
}