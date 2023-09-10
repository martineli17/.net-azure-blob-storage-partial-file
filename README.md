# Azure Blob Storage - Partial File
## Contexto
<p>Ao solicitar o retorno dos dados de um determinado arquivo, esse retorno pode ser particionado a fim de econimizar processamento e recursos.</p>
<p>Imagine o cenário em que exista um arquivo extenso e com grande volume de dados. Se o mesmo for carregado por completo de uma única vez na memória de sua aplicação, isso pode torná-la lenta durante o processamento e utilizar recursos além do necessário.</p>
<p>Ainda com esse cenário em mente e supondo que existe uma análise para verificar a consistência dos dados, se existir algum dado inconsistente logo no início do arquivo, o restante dos dados foi carregado e não será utilizado. Isso irá gastar recursos que não foram necessários ao longo do processo.</p>
<p>No Azure Blob Storage, existe uma maneira de solicitar partes específicas de um arquivo, carregando o conteúdo sob demanda.</p>

## Utilização
Neste exemplo, foi criada uma classe para centralizar o Upload e Download de arquivos: [BlobService](https://github.com/martineli17/.net-azure-blob-storage-partial-file/blob/master/Api/BlobService.cs)
- Para realizar o download parcial de um arquivo, foi utilizado o método 'DownloadContentAsync'. Neste método, ele recebe como parâmetro a classe 'BlobDownloadOptions', que contém uma propriedade responsável por determinar qual é o offset e o length que queremos retornar do arquivo informado. Essa propriedade é do type 'HttpRange'.
```csharp
public async Task<string> GetPartialFileAsync(long startAt, long endAt, string fileName, string containerName)
{
    var httpRange = new HttpRange(startAt, endAt);
    var containerClient = _blobClient.GetBlobContainerClient(containerName);
    var blobClient = containerClient.GetBlobClient(fileName);
    var contentFile = await blobClient.DownloadContentAsync(new BlobDownloadOptions()
    {
        Range = httpRange,
    });

    return Encoding.UTF8.GetString(contentFile.Value.Content);
}
```

Além disso, foi criado dois endpoints:
- Um endpoint para realizar o upload do arquivo default para testes.
- Um endpoint para testar o retorno parcial do arquivo, tendo como query params o offset e o length.

## Pacotes
- Azure.Storage.Blobs
