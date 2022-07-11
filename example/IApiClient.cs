using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OpenApiForge
{
    // Swagger Petstore - OpenAPI 3.0
    public interface IApiClient
    {
        // Update an existing pet
        // Update an existing pet by Id
        public Task<Pet> updatePet(Pet body);

        // Add a new pet to the store
        // Add a new pet to the store
        public Task<Pet> addPet(Pet body);

        // Finds Pets by status
        // Multiple status values can be provided with comma separated strings
        public Task<Pet[]> findPetsByStatus(string status = "available");

        // Finds Pets by tags
        // Multiple tags can be provided with comma separated strings. Use tag1, tag2, tag3 for testing.
        public Task<Pet[]> findPetsByTags(string[] tags);

        // Find pet by ID
        // Returns a single pet
        public Task<Pet> getPetById(long petId);

        // Updates a pet in the store with form data

        public Task updatePetWithForm(long petId, string name, string status);

        // Deletes a pet

        public Task deletePet(long petId, string api_key);

        // uploads an image

        public Task<ApiResponse> uploadFile(long petId, string additionalMetadata);

        // Returns pet inventories by status
        // Returns a map of status codes to quantities
        public Task<Dictionary<string, int>> getInventory();

        // Place an order for a pet
        // Place a new order in the store
        public Task<Order> placeOrder(Order body);

        // Find purchase order by ID
        // For valid response try integer IDs with value &lt;&#x3D; 5 or &gt; 10. Other values will generate exceptions.
        public Task<Order> getOrderById(long orderId);

        // Delete purchase order by ID
        // For valid response try integer IDs with value &lt; 1000. Anything above 1000 or nonintegers will generate API errors
        public Task deleteOrder(long orderId);

        // Create user
        // This can only be done by the logged in user.
        public Task<User> createUser(User body);

        // Creates list of users with given input array
        // Creates list of users with given input array
        public Task<User> createUsersWithListInput(User[] body);

        // Logs user into the system

        public Task<string> loginUser(string username, string password);

        // Logs out current logged in user session

        public Task logoutUser();

        // Get user by user name

        public Task<User> getUserByName(string username);

        // Update user
        // This can only be done by the logged in user.
        public Task updateUser(string username, User body);

        // Delete user
        // This can only be done by the logged in user.
        public Task deleteUser(string username);
    }
}