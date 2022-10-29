// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IVaultSecretClientConfig.cs" company="Orion Fusion Inc">
//   Copyright (c) 2022 Orion Fusion Inc. All Rights Reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace mykvscretclient
{
    public interface IVaultSecretClientConfig
    {
        string TenantId { get; }
        string KeyVaultUri { get; }
        string ClientId { get; }
        string CertificateThumbprint { get; }
    }
}