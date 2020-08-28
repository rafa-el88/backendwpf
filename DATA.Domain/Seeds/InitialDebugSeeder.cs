using DATA.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace DATA.Domain.Seeds
{
    public class InitialDebugSeeder
    {
        private Random gen = new Random();
        private string[] _nomes = "Joao,Gabriel,Lucas,Pedro,Mateus,Jose,Gustavo,Guilherme,Carlos,Vitor,Felipe,Marcos,Rafael,Luiz,Daniel,Eduardo,Matheus,Luis,Bruno,Paulo,Maria,Ana,Vitoria,Julia,Leticia,Amanda,Beatriz,Larissa,Gabriela,Mariana,Bruna,Camila,Isabela,Luana,Sara,Eduarda,Bianca,Rafaela,Geovana,Fernanda".Split(',');
        private AppDbContext _ctx; 
        public InitialDebugSeeder(AppDbContext appDBContext)
        {
            _ctx = appDBContext;
        }

        public void SeedData()
        {
            //_ctx.Users.RemoveRange(_ctx.Users.ToList());
            //_ctx.SaveChanges();
            //Se estivermos debugando e o bd estiver vazio, o seed será feito
#if DEBUG
            if (_ctx.Users.Count() == 0 && System.Diagnostics.Debugger.IsAttached)
            {
                Random rdm = new Random();
                for (int i = 0; i < 45; i++)
                {
                    User _usr = new User(); 
                    _usr.Nome = String.Format("{0}", _nomes[gen.Next(_nomes.Length - 1)], _nomes[gen.Next(_nomes.Length - 1)]);
                    _usr.Sobrenome = String.Format("{0}", _nomes[gen.Next(_nomes.Length - 1)]);
                    int c = rdm.Next(1000000, 99999999);
                    _usr.Telefone = c.ToString("0000-0000");
                    try
                    {
                        _ctx.Add(_usr); 
                    }catch (Exception){}
                } 
                _ctx.SaveChanges();
            }
#endif
        }  
    }
}
