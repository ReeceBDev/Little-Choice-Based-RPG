namespace Little_Choice_Based_RPG.Managers.Server
{
    internal static class SessionManager
    {
        //Maybe player sessions also have their playerlog held here on the game-side. That way, if a player disconnects and reconnects in short span, their session is not lost.
        //Sessions should obviously expire, but I'd like for them to stick around a little bit.
        //That way if a player crashes, accidentally closes, or anything else, they can just resume exactly where they left off.
        //This could also maybe hold their current screen, and any other transient data, which could get re-constructed.
        //I like it, not a bad idea.
        //Alternatively, that's what cookies are for! But they dont work on the console thing, like I found out!
    }
}
