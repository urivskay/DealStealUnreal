using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace DealStealUnreal
{
    public class DSURepository: DbContext
    {
        #region Properties
        
        public DbSet<User>    Users     { get; set; }
        public DbSet<Role>    Roles     { get; set; }
        public DbSet<Deal>    Deals     { get; set; }
        public DbSet<Vote>    Votes     { get; set; }
        public DbSet<Gift>    Gifts     { get; set; }
        public DbSet<Mailing> Mailings  { get; set; }
        public DbSet<Comment> Comments  { get; set; }


        private const string MissingRole  = "Role does not exist";
        private const string MissingUser  = "User does not exist";
        private const string TooManyUser  = "User already exists";
        private const string TooManyRole  = "Role already exists";
        private const string AssignedRole = "Cannot delete a role with assigned users";
        #endregion

        public DSURepository()
            : base("DefaultConnection")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Console.WriteLine("OnModelCreating");
            modelBuilder.Entity<Vote>()
                .HasRequired(t => t.Deal)
                .WithMany(w => w.Votes)
                .HasForeignKey(t => t.DealId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Comment>()
                .HasRequired(t => t.Deal)
                .WithMany(w => w.Comments)
                .HasForeignKey(t => t.DealId)
                .WillCascadeOnDelete(false);
        }

        #region Select Methods

        public IQueryable<User> GetAllUsers()
        {
            return from user in Users
                   orderby user.UserName
                   select user;
        }

        public User GetUser(string NameOrEmail)
        {
            return Users.SingleOrDefault(user => user.UserName == NameOrEmail || user.Email == NameOrEmail);
        }

        public ICollection<Role> GetRoleForUser(int userId)
        {
            return Users.SingleOrDefault(user => user.Id == userId).Roles;
        }

        public ICollection<Role> GetRolesForUser(string userNameOrEmail)
        {
            User user = Users.SingleOrDefault(_user => _user.UserName == userNameOrEmail || _user.Email == userNameOrEmail);
            return user == null ? new List<Role>() : user.Roles;
        }

        public Role GetRole(string roleName)
        {
            return Roles.SingleOrDefault(role => role.Name == roleName);
        }
        public bool UserExists(User user)
        {
            return (Users.SingleOrDefault(u => u.Id == user.Id || u.UserName == user.UserName) != null);
        }
        #endregion

        #region Insert/Delete Methods
        private void AddUser(User user)
        {
            if (UserExists(user))
                throw new ArgumentException(TooManyUser);

            Users.Add(user);
        }

        public void CreateUser(string username, string password, string email, string roleName)
        {
            Role role = GetRole(roleName);

            if (string.IsNullOrEmpty(username.Trim()))
                throw new ArgumentException("The user name provided is invalid. Please check the value and try again.");
 
            if (string.IsNullOrEmpty(password.Trim()))
                throw new ArgumentException("The password provided is invalid. Please enter a valid password value.");

            if (string.IsNullOrEmpty(email.Trim()))
                throw new ArgumentException("The e-mail address provided is invalid. Please check the value and try again.");

            if (this.Users.Any(user => user.UserName == username))
                throw new ArgumentException("Username already exists. Please enter a different user name.");

            if (this.Users.Any(user => user.Email == email))
                throw new ArgumentException("User with that email is already registered.");

            User newUser = new User()
            {
                UserName = username,
                Password = Security.Hash.HashString(password.Trim()),
                Email = email,
                RoleId = role.Id
            };

            try
            {
                AddUser(newUser);
            }
            catch (ArgumentException ae)
            {
                throw ae;
            }
            catch (Exception e)
            {
                throw new ArgumentException("The authentication provider returned an error. Please verify your entry and try again. " +
                    "If the problem persists, please contact your system administrator.",e);
            }

            // Immediately persist the user data
            this.SaveChanges();
        }

        public void DeleteUser(string NameOrEmail)
        {
            Users.Remove(GetUser(NameOrEmail));
            this.SaveChanges();
        }

        #endregion

    }

    // class create Database if Entities was editing.
    public class RepositoryInitializer : DropCreateDatabaseIfModelChanges<DSURepository>
    {
        protected override void Seed(DSURepository context)
        {
            List<Role> roles = new List<Role>
            {
                new Role { Name = "Admin"  },
                new Role { Name = "Manager"},
                new Role { Name = "User"   }
            };

            User admin = new User
            {
                UserName = "admin",
                Password = Security.Hash.HashString("pings3-brief"),
                Email = "dealstealunreal@gmail.com"
                
            };

            context.Users.Add(admin);

            // add data into context and save to db
            foreach (Role r in roles)
            {
                context.Roles.Add(r);                
            }

            context.SaveChanges();
        }
    }

    // class create Database if Entities was editing.
    public class RepositoryInitializer2 : CreateDatabaseIfNotExists<DSURepository>
    {
        protected override void Seed(DSURepository context)
        {
            List<Role> roles = new List<Role>
            {
                new Role { Name = "Admin"  },
                new Role { Name = "Manager"},
                new Role { Name = "User"   }
            };

            User admin = new User
            {
                UserName = "admin",
                Password = Security.Hash.HashString("pings3-brief"),
                Email = "dealstealunreal@gmail.com"

            };

            context.Users.Add(admin);

            // add data into context and save to db
            foreach (Role r in roles)
            {
                context.Roles.Add(r);
            }

            context.SaveChanges();
        }
    }


    #region Entities

    public class User
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int    Id             { get; set; }
        [Required]
        [MaxLength(50)]
        public string UserName       { get; set; }
        [Required]
        [MaxLength(50)]
        public string Email          { get; set; }
        [Required]
        [MaxLength(50)]
        public string Password       { get; set; }
        public string ProfilePicture { get; set; }
        public string Points         { get; set; }
        public long   FacebookId     { get; set; }
        public int    RoleId         { get; set; }

        public virtual ICollection<Role>    Roles        { get; set; }
        public virtual ICollection<Deal>    Deals        { get; set; }
        public virtual ICollection<Comment> Comments     { get; set; }
        public virtual ICollection<Vote>    Votes        { get; set; }

        public User()
        {
            this.Roles = new List<Role>();
        }
    }

    public class Gift
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int    Id          { get; set; }
        public int    countDeal   { get; set; }
        public int    countSteal  { get; set; }
        public int    countUnreal { get; set; }
        public string image       { get; set; }
        public string description { get; set; }
    }

    public class Mailing
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int      Id      { get; set; }
        public string   message { get; set; }
        public DateTime date    { get; set; }
        [Required]
        public string   status  { get; set; }
    }

    public class Role
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int    Id   { get; set; }
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        public virtual ICollection<User> Users { get; set; }

        public Role()
        {
            this.Users = new List<User>();
        }
    }

    public class Comment
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int      Id        { get; set; }
        [Required]
        public int      UserId    { get; set; }
        [Required]
        public int      DealId    { get; set; }
        [Required]
        public string   Text      { get; set; }
        [Required]
        public DateTime Date      { get; set; }

        public virtual  User User { get; set; }
        public virtual  Deal Deal { get; set; }
    }

    public class Deal
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int      Id          { get; set; }
        public int      UserId      { get; set; }
        [Required]
        [MaxLength(50)]
        public string   Title       { get; set; }
        public string   Description { get; set; }
        public string   Retailer    { get; set; }
        public string   Url         { get; set; }
        [Required]
        public string   Price       { get; set; }
        public string   ImgUrl      { get; set; }
        public bool     Active      { get; set; }
        [Required]
        public DateTime Date        { get; set; }

        public virtual User User    { get; set; }
        public virtual ICollection<Vote>    Votes    { get;  set; }
        public virtual ICollection<Comment> Comments { get;  set; }
    }

    public class Vote
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id     { get; set; }
        public int UserId { get; set; }
        public int DealId { get; set; }
        [Required]
        public int Votes  { get; set; }
        public virtual User User { get; set; }
        public virtual Deal Deal { get; set; }
    }
    #endregion
   
}