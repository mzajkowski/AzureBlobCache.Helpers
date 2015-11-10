AzureBlobCache Helpers for Umbraco
============
**Using the Azure Blob Cache plugin for ImageProcessor and this helper you can render fully qualified CDN urls directly in your Views instead of ImageProcessor issuing a 302 redirect**

This helper adds an overloaded `GetCropUrl` method with the additional parameter `resolveCdnPath` e.g.` mediaItem.GetCropUrl(width:277, height:100, resolveCdnPath:true)`

**Note**: your server must be able to resolve it's own assigned domains for this to work correctly. If you don't have loopback on your IIS servers you can workaround this by using the hosts file on the servers.