using NerdStore.Core.DomainObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NerdStore.Catalogo.Domain
{
    public class Produto : Entity, IAggregateRoot
    {
        public Guid CategoriaId { get; private set; }
        public string Nome { get; private set; }
        public string Descricao { get; private set; }
        public bool Ativo { get; private set; }
        public decimal Valor { get; private set; }
        public DateTime DataCadastro { get; private set; }
        public string Imagem { get; private set; }
        public decimal QuantidadeEstoque { get; private set; }
        public Dimensoes Dimensoes { get; protected set; }
        public Categoria Categoria { get; private set; }

        public Produto(string nome, string descricao, bool ativo, decimal valor, Guid categoriaId, DateTime dataCadastro, string imagem, Dimensoes dimensoes)
        {
            this.CategoriaId = categoriaId;
            this.Nome = nome;
            this.Descricao = descricao;
            this.Ativo = ativo;
            this.Valor = valor;
            this.DataCadastro = dataCadastro;
            this.Imagem = imagem;
            this.Dimensoes = dimensoes;

            Validar();
        }

        public void Ativar() => Ativo = true;
        public void Desativar() => Ativo = false;

        public void AlterarCategoria(Categoria categoria)
        {
            this.Categoria = categoria;
            this.CategoriaId = categoria.Id;
        }

        public void AlterarDescricao(string descricao)
        {
            Validacoes.ValidarSeVazio(descricao, "O campo Descricao do produto não pode estar vazio");
            this.Descricao = descricao;
        }

        public void DebitarEstoque(int quantidade)
        {
            if (quantidade < 0) quantidade *= -1;
            if (!PossuiEstoque(quantidade)) throw new DomainException("Estoque insuficiente");

            this.QuantidadeEstoque -= quantidade;
        }

        public void ReporEstoque(decimal quantidade)
        {
            this.QuantidadeEstoque += quantidade;
        }

        public bool PossuiEstoque(decimal quantidade)
        {
            return this.QuantidadeEstoque >= quantidade;
        }

        public void Validar()
        {
            Validacoes.ValidarSeVazio(Nome, "O campo Nome do produto não pode estar vazio");
            Validacoes.ValidarSeVazio(Descricao, "O campo Descricao do produto não pode estar vazio");
            Validacoes.ValidarSeDiferente(CategoriaId, Guid.Empty, "O campo CategoriaId do produto não pode estar vazio");
            Validacoes.ValidarSeMenorQue(Valor, 1, "O campo Valor do produto não pode ser menor igual a 0");
            Validacoes.ValidarSeVazio(Imagem, "O campo Imagem do produto não pode estar vazio");
        }
    }

    public class Categoria : Entity
    {
        public string Nome { get; private set; }
        public int Codigo { get; private set; }

        public Categoria(string nome, int codigo)
        {
            this.Nome = nome;
            this.Codigo = codigo;
        }

        public override string ToString()
        {
            return $"{this.Nome} - {this.Codigo}";
        }
    }
}
