# TFG
Trabalho final de graduação - Orientado pelo Dr. Alexandre de O. Zamberlan

# Documentação: Integração do Unity com Jacamo

Este documento fornece um guia passo a passo para instalar e executar o sistema que integra o Unity com o Jacamo.

## Pré-requisitos

Antes de iniciar, certifique-se de ter os seguintes itens instalados em seu sistema:

- **Unity**: Versão recomendada 2020.3 LTS ou superior.
- **Jacamo**: Framework para agentes inteligentes. (https://jacamo-lang.github.io/jacamo/install.html)
- **Java Development Kit (JDK)**: Versão 17.
- **Maven**: Para gerenciamento de dependências do Jacamo.

## Passo 1: Clonar o Repositório

Clone o repositório do GitHub para obter os arquivos necessários:
```bash
git clone https://github.com/bernardoviero/TFG2.git
```

## Passo 2: Configurar o Ambiente Java

1. **Instale o JDK**: Baixe e instale a versão adequada do JDK a partir do site oficial.
2. **Configure as variáveis de ambiente**:
   - `JAVA_HOME`: Aponte para o diretório de instalação do JDK.
   - `PATH`: Inclua o diretório bin do JDK para acesso aos comandos Java no terminal.

## Passo 3: Instalar o Maven

1. **Baixe o Maven**: Acesse o site oficial e baixe a versão mais recente.
2. **Configure as variáveis de ambiente**:
   - `MAVEN_HOME`: Aponte para o diretório de instalação do Maven.
   - `PATH`: Inclua o diretório bin do Maven para acesso aos comandos Maven no terminal.

## Passo 4: Compilar o Jacamo

1. Navegue até o diretório do Jacamo dentro do repositório clonado:

    ```bash
    cd TFG2/Jacamo
    ```

2. Compile o Jacamo utilizando o Maven:

    ```bash
    mvn clean install
    ```

Isso baixará as dependências necessárias e compilará o Jacamo.

## Passo 5: Configurar o Unity

1. **Instale o Unity**: Baixe e instale a versão recomendada do Unity Hub a partir do site oficial.
2. **Crie um Novo Projeto**: Abra o Unity Hub, clique em "Novo" e selecione um template adequado para o projeto.
3. **Importe o Jacamo para o Unity**:
   - No Unity, vá até `Assets > Import Package > Custom Package`.
   - Selecione o arquivo `.unitypackage` do Jacamo que foi compilado anteriormente.

## Passo 6: Integrar o Jacamo no Unity

1. **Configurar o Agente Jacamo**:
   - Crie um novo GameObject no Unity.
   - Adicione o componente `JacamoAgent` a esse GameObject.
   - Configure as propriedades do agente conforme necessário.

2. **Desenvolver o Comportamento do Agente**:
   - Escreva scripts em Java que definam o comportamento do agente.
   - Compile esses scripts e certifique-se de que estão acessíveis pelo Unity.

## Passo 7: Executar o Sistema

1. **Inicie o Servidor Jacamo**:
   - No terminal, navegue até o diretório onde o Jacamo foi compilado.
   - Execute o comando:

    ```bash
    jacamo air-traffic-mas.jcm
    ```
