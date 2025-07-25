using Bogus;
using System;
using TaskMeUp.Api.DAL;
using TaskMeUp.Api.Entities;
using Task = TaskMeUp.Api.Entities.Task;
using TaskStatus = TaskMeUp.Api.Entities.TaskStatus;

namespace TaskMeUp.Api
{
    public static class DatabaseSeeder
    {
        private static readonly string[] PortraitList = [
        "Asset 1.png",
        "Asset 2.png",
        "Asset 3.png",
        "Asset 4.png",
        "Asset 5.png",
        "Asset 6.png"
    ];

        public static void Seed(MainDbContext context)
        {
            if (context.Users.Any() || context.Groups.Any() || context.Tasks.Any())
                return;

            var faker = new Faker("en");

            // Generate Users
            var users = new Faker<User>()
                .RuleFor(u => u.Id, _ => Guid.NewGuid())
                .RuleFor(u => u.Username, f => f.Internet.UserName())
                .RuleFor(u => u.PasswordHash, _ => "fakehash")
                .RuleFor(u => u.Portrait, f => f.PickRandom(PortraitList))
                .Generate(25);

            var adminUser = new User()
            {
                Username = "admin",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("admin"),
                Id = Guid.NewGuid(),
                Portrait = "Asset 1.png"
            };
            users.Add(adminUser);

            // Generate Groups and assign users
            var groups = new List<Group>();
            for (int i = 0; i < 15; i++)
            {
                var groupUsers = faker.PickRandom(users, faker.Random.Int(2, 5)).ToList();
                groupUsers.Add(adminUser);
                var group = new Group
                {
                    Id = Guid.NewGuid(),
                    Name = faker.Commerce.Department(),
                    Description = faker.Lorem.Paragraph(),
                    Icon = $"icon-{i + 1}",
                    Tags = faker.Lorem.Words(faker.Random.Int(2, 5)),
                    Users = groupUsers,
                    Tasks = new List<Task>() 
                };
                groups.Add(group);
            }

            // Link users to groups (bi-directionally)
            foreach (var user in users)
            {
                var userGroups = groups.Where(g => g.Users.Contains(user)).ToList();
                user.Groups = userGroups;
            }

            // Generate Tasks and assign to groups/users
            var allTasks = new List<Task>();
            foreach (var group in groups)
            {
                var groupTasks = new Faker<Task>()
                    .RuleFor(t => t.Id, _ => Guid.NewGuid())
                    .RuleFor(t => t.Title, f => f.Lorem.Sentence(4))
                    .RuleFor(t => t.Description, f => f.Lorem.Paragraph())
                    .RuleFor(t => t.Status, f => f.PickRandom<TaskStatus>())
                    .RuleFor(t => t.DueDate, f => f.Date.Soon(30))
                    .RuleFor(t => t.CreatedAt, f => f.Date.Recent(10))
                    .RuleFor(t => t.Group, _ => group)
                    .RuleFor(t => t.AssignedUser, f => f.PickRandom(group.Users.ToList()))
                    .Generate(faker.Random.Int(10,30));

                ((List<Task>)group.Tasks).AddRange(groupTasks);
                allTasks.AddRange(groupTasks);
            }

            // Add to context
            context.Users.AddRange(users);
            context.Groups.AddRange(groups);
            context.Tasks.AddRange(allTasks);

            context.SaveChanges();
        }
    }
}
