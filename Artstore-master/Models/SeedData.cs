using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;
using ArtStore.Data;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace ArtStore.Models
{
    public class SeedData
    {
        public static async Task CreateUserRoles(IServiceProvider serviceProvider)
        {
            var RoleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var UserManager = serviceProvider.GetRequiredService<UserManager<AppUser>>();

            IdentityResult roleResult;
            //Adding Admin Role
            var roleCheck = await RoleManager.RoleExistsAsync("Admin");
            if (!roleCheck)
            {
                roleResult = await RoleManager.CreateAsync(new IdentityRole("Admin"));
            }
            roleCheck = await RoleManager.RoleExistsAsync("User");
            if (!roleCheck)
            {
                roleResult = await RoleManager.CreateAsync(new IdentityRole("User"));
            }


            AppUser user = await UserManager.FindByEmailAsync("artistik@gmail.com");
            if (user == null)
            {
                var User = new AppUser();
                User.Email = "artistik@gmail.com";
                User.UserName = "Artistik";
                User.Role = "Admin";
                string userPWD = "Admin123@";
                IdentityResult chkUser = await UserManager.CreateAsync(User, userPWD);
                //Add default User to Role Admin      
                if (chkUser.Succeeded)
                {
                    var result1 = await UserManager.AddToRoleAsync(User, "Admin");
                }
            }
        }

        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new ArtstoreContext(
                serviceProvider.GetRequiredService<
                    DbContextOptions<ArtstoreContext>>()))
            {
                CreateUserRoles(serviceProvider).Wait();
                // Look for any movies.
                if (context.Painting.Any() || context.Painter.Any() || context.User.Any())
                {
                    return;   // DB has been seeded
                }
                context.Painter.AddRange(
                    new Painter
                    {
                        Picture = "leonardo-da-vinci.jpg",
                        FirstName = "Леонардо",
                        LastName = "да Винчи",
                        DateofBirth = DateTime.Parse("1452-4-15"),
                        DateofDeath = DateTime.Parse("1519-5-2"),
                        Biography = "Познат италијански ренесансен архитект, инженер, вајар, уметник, сликар, изумител, анатом, ботаничар и астроном. Tој сакал да знае сè за природата, односно како таа работи. Леонардо бил многу добар во проучување, дизајнирање и изработка на секакви интересни работи. Најпознат е по неговите слики како Мона Лиза, Тајната вечера и Витрувиевот човек",
                        Paintings = "Мона Лиза"
                    },
                     new Painter
                     {
                         Picture = "Gustav_Klimt.jpg",
                         FirstName = "Густав",
                         LastName = "Климт",
                         DateofBirth = DateTime.Parse("1862-7-14"),
                         DateofDeath = DateTime.Parse("1918-2-6"),
                         Biography = "Тој е австриски сликар од симболизмот и еден од најпроминентните членови на движењето на Виенската сецесија. Климт е познат по своите слики, мурали, скици и други уметнички дела. Основниот објект на Климт е женското тело, а во неговите дела се забележува директен еротицизам.",
                         Paintings = "Бакнеж"
                     },
                        new Painter
                        {
                            Picture = "Pablo_picasso.jpg",
                            FirstName = "Пабло",
                            LastName = "Пикасо",
                            DateofBirth = DateTime.Parse("1881-10-25"),
                            DateofDeath = DateTime.Parse("1973-4-8"),
                            Biography = "Беше шпански сликар, скулптор, графичар, керамичар и театарски дизајнер кој поголемиот дел од својот возрасен живот го поминал во Франција. Еден од највлијателните уметници на 20 век, тој е познат по ко-основачот на кубистичкото движење, пронајдокот на конструирана скулптура, ко-промислувањето на колажот и по широката разновидност на стилови кои тој помогнал да се развијат и истражат. Меѓу неговите најпознати дела се протокубистичките Les Demoiselles d'Avignon (1907) и антивоената слика Герника (1937), драматичен приказ на бомбардирањето на Герника од страна на германските и италијанските воздухопловни сили за време на Шпанската граѓанска војна.",
                            Paintings = "Герника"
                        },
                        new Painter
                        {
                            Picture = "Johannes_Vermeer.jpg",
                            FirstName = "Јоханес",
                            LastName = "Вермер",
                            DateofBirth = DateTime.Parse("1632-10-31"),
                            DateofDeath = DateTime.Parse("1675-12-15"),
                            Biography = "Познат и како Јан Вермер бил холандски сликар од барокниот период кој специјализирал за домашни внатрешни сцени од животот на средната класа. За време на неговиот живот, тој бил умерено успешен провинциски жанровски сликар, признат во Делфт и Хаг. Сепак, тој направил релативно малку слики и очигледно не бил богат, оставајќи ги сопругата и децата во долгови при неговата смрт.",
                            Paintings = "Девојка со бисерна обетка"
                        },
                       new Painter
                       {
                           Picture = "Vincent_van_Gogh.jpg",
                           FirstName = "Винсент",
                           LastName = "ван Гог",
                           DateofBirth = DateTime.Parse("1853-3-30"),
                           DateofDeath = DateTime.Parse("1890-7-29"),
                           Biography = "Бил холандски постимпресионистички сликар, чии дела имале големо влијание врз уметноста во XX век, поради живите бои и отсликувањето на чувствата. Боледувал од анксиозност и имал чести напади на ментална болест, а починал на 37-годишна возраст, извршувајќи самоубиство.",
                           Paintings = "Ѕвездена ноќ"
                       },
                       new Painter
                       {
                           Picture = "Jan_van_Eyck.jpg",
                           FirstName = "Јан ван",
                           LastName = "Ејк",
                           DateofBirth = DateTime.Parse("1390-1-1"),
                           DateofDeath = DateTime.Parse("1441-7-9"),
                           Biography = "Бил активен сликар и еден од раните иноватори на она што стана познато како ранохоландско сликарство и еден од најзначајните претставници на уметноста на раната северна ренесанса. Според Вазари и други историчари на уметност, вклучително и Ернст Гомбрих, тој го измислил сликарството во масло.",
                           Paintings = "Портрет на Арнолфиниеви"
                       }
                );
                context.SaveChanges();

                context.Painting.AddRange(
                    new Painting
                    {
                        Picture = "mona-lisa.jpg",
                        PainterID = context.Painter.Single(d => d.FirstName == "Леонардо" && d.LastName == "да Винчи").PainterID,
                        Title = "Мона Лиза",
                        OriginalTitle = "Mona Lisa",
                        Category = "Портрет",
                        Description = "Мона Лиза е слика во масло од италијанскиот уметник, пронаоѓач и писател Леонардо да Винчи. Веројатно завршено во 1506 година, делото содржи портрет на седечка жена поставена на замислен пејзаж. Покрај тоа што е една од најпознатите слики, таа е и највредна.",
                        Price = 456568048,
                        ReleaseDate = DateTime.Parse("1503-1-1")
                    },
                     new Painting
                     {
                         Picture = "The-Kiss.jpg",
                         PainterID = context.Painter.Single(d => d.FirstName == "Густав" && d.LastName == "Климт").PainterID,
                         Title = "Бакнеж",
                         OriginalTitle = "The Kiss",
                         Category = "Битова уметност",
                         Description = "Бакнеж е слика со масло на платно со додадени златни листови, сребро и платина од австрискиот симболист сликар Густав Климт. Тој бил насликан во одреден момент во 1907 и 1908 година, за време на екот на она што научниците го нарекуваат неговиот „Златен период“.",
                         Price = 13793592,
                         ReleaseDate = DateTime.Parse("1908-1-1")
                     },
                     new Painting
                     {
                         Picture = "Guernica.jpg",
                         PainterID = context.Painter.Single(d => d.FirstName == "Пабло" && d.LastName == "Пикасо").PainterID,
                         Title = "Герника",
                         OriginalTitle = "Guernica",
                         Category = "Историско сликарство",
                         Description = "Герника е голема слика во масло од 1937 година на шпанскиот уметник Пабло Пикасо. Тоа е едно од неговите најпознати дела, кое многу ликовни критичари го сметаат за најтрогателната и најмоќната антивоена слика во историјата. Изложена е во Музејот Реина Софија во Мадрид.",
                         Price = 1230687986,
                         ReleaseDate = DateTime.Parse("1937-7-1")
                     },
                       new Painting
                       {
                           Picture = "Girl_with_a_Pearl_Earring.jpg",
                           PainterID = context.Painter.Single(d => d.FirstName == "Јоханес" && d.LastName == "Вармер").PainterID,
                           Title = "Девојката со бисерна обетка",
                           OriginalTitle = "Girl with a pearl earring",
                           Category = "Трони",
                           Description = "Девојка со бисерна обетка е слика во масло на холандскиот сликар од златното доба Јоханес Вермер, датирана в. 1665. Со различни имиња низ вековите, таа стана позната по денешниот наслов кон крајот на 20 век по обетката што ја носеше девојката прикажана таму.",
                           Price = 350345234,
                           ReleaseDate = DateTime.Parse("1665-1-1")
                       },
                    new Painting
                    {
                        Picture = "Starry_Night.jpg",
                        PainterID = context.Painter.Single(d => d.FirstName == "Винсент" && d.LastName == "ван Гог").PainterID,
                        Title = "Ѕвездена ноќ",
                        OriginalTitle = "Тhe Starry Night",
                        Category = "Пејзажно сликарство",
                        Description = "Ѕвездената ноќ е слика со масло на платно на холандскиот пост-импресионистички сликар Винсент ван Гог. Насликан во јуни 1889 година, го прикажува погледот од прозорецот свртен кон исток на неговата соба за азил во Сен Реми-де-Прованс, непосредно пред изгрејсонце, со додавање на имагинарно село.",
                        Price = 592124520,
                        ReleaseDate = DateTime.Parse("1889-6-1")
                    },
                    new Painting
                    {
                        Picture = "Arnolfini_Portrait.jpg",
                        PainterID = context.Painter.Single(d => d.FirstName == "Јан ван" && d.LastName == "Ејк").PainterID,
                        Title = "Портрет на Арнолфиниеви",
                        OriginalTitle = "Arnolfini Portrait",
                        Category = "Портрет",
                        Description = "Портретот на Арнолфини е сликарство во масло од 1434 година на дабова плоча од раниот холандски сликар Јан ван Ајк. Формира двоен портрет со целосна должина, за кој се верува дека ги прикажува италијанскиот трговец Џовани ди Николао Арнолфини и неговата сопруга, веројатно во нивната резиденција во фламанскиот град Бриж. ",
                        Price = 691403224,
                        ReleaseDate = DateTime.Parse("1434-1-1")
                    }
                );
                context.SaveChanges();
   

            }
        }
    }
}
