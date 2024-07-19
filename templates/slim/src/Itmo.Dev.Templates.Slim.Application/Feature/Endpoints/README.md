# Endpoints 

Every HTTP operation is implemented as [FastEndpoints](https://github.com/FastEndpoints/FastEndpoints) handler.

As this is a slim (simple) template, you should implement 
all business logic in these handlers.

If you need to reuse some complex logic, you may create a Services folder,
but be aware that presence of complex logic may suggest that this template 
is invalid for the usecase (you may consider itmo-dev-hexagonal template).

See [FastEndpoints docs](https://fast-endpoints.com/docs/get-started#cancellation-token) for more implementation details.