using Little_Choice_Based_RPG.Managers.Player_Manager.Frontend.UserInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Little_Choice_Based_RPG.Types.MenuElements
{
    /// <summary> Provides a list of Menu Elements with custom functionality. </summary>
    public class MenuElementList
    {
        public void UpsertElement(uint priority, MenuElementIdentity identity, string content, IUserInterface sourceMenu)
        {
            //If the identity already exists, remove it first
            foreach (var target in elements.ToList())
            {
                if (target.UniqueIdentity == identity)
                    elements.Remove(target);
                break;
            }

            //Add the new element
            elements.Add(new MenuElement(priority, identity, content, 0, sourceMenu));
        }

        public void ChangePriority(uint newPriority, MenuElementIdentity identity, IUserInterface sourceMenu)
        {
            bool identityFound = false;
            
            //If the identity already exists, replace it
            foreach (var target in elements.ToList())
            {
                if (target.UniqueIdentity == identity)
                {
                    elements.Remove(target);
                    elements.Add(new MenuElement(newPriority, identity, target.Content, 0, sourceMenu));

                    identityFound = true;
                    break;
                }
            }

            if (!identityFound)
                throw new Exception("Could not change the priority. The target identity {identity} was not found!");
        }

        public List<MenuElement> elements { get; private set; } = new();
    }
}