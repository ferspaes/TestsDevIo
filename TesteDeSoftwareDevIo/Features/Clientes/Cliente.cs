﻿using Features.Core;
using FluentValidation;
using System;

namespace Features.Clientes
{
    public class Cliente : Entity
    {
        public string Nome { get; set; }
        public string SobreNome { get; set; }
        public DateTime DataNascimento { get; set; }
        public DateTime DataCadastro { get; set; }
        public string Email { get; set; }
        public bool Ativo { get; set; }

        protected Cliente() { }

        public Cliente(Guid id, string nome, string sobrenome, DateTime dataNascimento, string email, bool ativo, DateTime dataCadastro)
        {
            Id = id;
            Nome = nome;
            SobreNome = sobrenome;
            DataNascimento = dataNascimento;
            Email = email;
            Ativo = ativo;
            DataCadastro = dataCadastro;
        }

        public string NomeCompleto() =>
            $"{Nome} {SobreNome}";

        public bool EhEspecial() =>
            DataCadastro < DateTime.Now.AddYears(-3) && Ativo;

        public void Inativar()
        {
            Ativo = false;
        }

        public override bool EhValido()
        {
            ValidationResult = new ClienteValidacao().Validate(this);
            return ValidationResult.IsValid;
        }
    }

    public class ClienteValidacao : AbstractValidator<Cliente>
    {
        public ClienteValidacao()
        {
            RuleFor(c => c.Nome)
                .NotEmpty().WithMessage("Por favor, certifique-se de ter inserido o nome.")
                .Length(2, 150).WithMessage("O nome deve ter entre 2 e 150 caracteres.");

            RuleFor(c => c.SobreNome)
                .NotEmpty().WithMessage("Por favor, certifique-se de ter inserido o sobrenome.")
                .Length(2, 150).WithMessage("O sobrenome deve ter entre 2 e 150 caracteres.");

            RuleFor(c => c.DataNascimento)
                .NotEmpty()
                .Must(HaveMinimumAge)
                .WithMessage("O cliente deve ter 18 anos ou mais.");

            RuleFor(c => c.Email)
                .NotEmpty()
                .EmailAddress();

            RuleFor(c => c.Id)
                .NotEqual(Guid.Empty);
        }

        private bool HaveMinimumAge(DateTime birthDate) =>
            birthDate <= DateTime.Now.AddYears(-18);
    }
}
