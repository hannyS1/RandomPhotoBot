namespace RandomPhotoBot;

public class UserMenuStateProvider
{
    private List<CurrentMenuState> _states = new()
    {
        new CurrentMenuState{UserId = "797712942", State = MenuState.Default}
    };
    
    public MenuState GetState(string userId)
    {
        var userState = _states.Find(s => s.UserId == userId);
        if (userState == null)
            throw new Exception("user state not found");

        return userState.State;
    }

    public void SetState(string userId, MenuState state)
    {
        var userState = _states.Find(s => s.UserId == userId);

        if (userState != null)
        {
            userState.State = state;
            return;
        }
        
        _states.Add(new CurrentMenuState{UserId = userId, State = state});
            
    }
}