using Application.Interfaces;

namespace Application.Movies.Commands.DeleteMovie;

public record DeleteMovieCommand(string Id) : IRequest;

public class DeleteMovieCommandHandler : IRequestHandler<DeleteMovieCommand>
{
    private readonly IApplicationDbContext _context;
    public DeleteMovieCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }
    public async Task Handle(DeleteMovieCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Movies.FindAsync(request.Id, cancellationToken);
        if(entity != null)
        {
            _context.Movies.Remove(entity);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}

