using RestaurantAPI.Entities;

namespace RestaurantAPI
{
    public class RestaurantSeeder
    {
        private readonly RestaurantDbContext _dbcontext;
        public RestaurantSeeder(RestaurantDbContext dbContext)
        {
            _dbcontext = dbContext;
        }
        public void Seed()
        {
            if (_dbcontext.Database.CanConnect())
            {
                if (!_dbcontext.Roles.Any())
                {
                   var roles = GetRoles();
                    _dbcontext.Roles.AddRange(roles);
                    _dbcontext.SaveChanges();
                }

                if (!_dbcontext.Restaurants.Any())
                {
                    var restaurants = GetRestaurants();
                    _dbcontext.Restaurants.AddRange(restaurants);

                    _dbcontext.SaveChanges();
                }
            }
        }
        private IEnumerable<Role> GetRoles()
        {
            var roles = new List<Role>()
            {
                new Role()
                {
                    Name = "User"
                },
                new Role()
                {
                    Name = "Admin"
                },
                new Role()
                {
                    Name = "Menager"
                },
            };
            return roles;
        }

        private IEnumerable<Restaurant> GetRestaurants()
        {
            var restaurants = new List<Restaurant>
            {
                new Restaurant
                {
                    Name = "KFC",
                    Category = "Fast Food",
                    Description = "KFC (short for Kentucky Fried Chicken) is an American fast food restaurant chain headquartered in Louisville, Kentucky, that specializes in fried chicken.",
                    ContactEmail = "kfc@gmail.com",
                    ContactNumber = "1231242355",
                    HasDelivery = true,
                    Dishes = new List<Dish>()
                    {
                        new Dish
                        {
                            Name = "Chicken Wings",
                            Price = 12.5M
                        },
                        new Dish
                        {
                            Name = "Chicken Drumsticks",
                            Price = 8.5M
                        }
                    },
                    Address = new Address
                    {
                        City = "New York",
                        Street = "Broadway",
                        PostalCode = "10001"
                    }
                },
                new Restaurant
                {
                    Name = "McDonalds",
                    Category = "Fast Food",
                    Description = "McDonald's Corporation is an American fast food company, founded in 1940 as a restaurant operated by Richard and Maurice McDonald, in San Bernardino, California, United States.",
                    ContactEmail = "mcDonalds@gmail.com",
                    ContactNumber = "2342852340",
                    HasDelivery = true,
                    Dishes = new List<Dish>()
                    {
                        new Dish
                        {
                            Name = "Big Mac",
                            Price = 5.5M
                        },
                        new Dish
                        {
                            Name = "Cheese Burger",
                            Price = 3.5M
                        }
                    },
                    Address = new Address
                    {
                        City = "New York",
                        Street = "Broadway",
                        PostalCode = "10001"
                    }
                },
                new Restaurant
                {
                    Name = "Pizza Hut",
                    Category = "Fast Food",
                    Description = "Pizza Hut is an American restaurant chain and international franchise which was founded in 1958 in Wichita, Kansas by Dan and Frank Carney.",
                    ContactEmail = "PizzaHut@gmail.com",
                    ContactNumber = "05349327423",
                    HasDelivery = true,
                    Dishes = new List<Dish>()
                    {
                        new Dish
                        {
                            Name = "Margherita",
                            Price = 7.5M
                        },
                        new Dish
                        {
                            Name = "Hawaiian",
                            Price = 9.5M
                        }
                    },
                    Address = new Address 
                    { 
                        City = "New York", 
                        Street = "Broadway", 
                        PostalCode = "10001" 
                    }
                }
            };
            return restaurants;
        }
    }
}
            
       

    

