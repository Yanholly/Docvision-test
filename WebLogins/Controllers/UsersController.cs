using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebLogins.Models;
using WebLogins.Processors;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace WebLogins.Controllers
{
    public class UsersController : ApiController
    {
        [HttpPost]
        [Route("UserRegister")]
        public int SaveUser(MyUser myUser)
        {
            if(myUser == null)
            {
                return 0;
            }
            return ContentProcessor.RegisterNewUser(myUser);
        }

        [HttpPost]
        [Route("UserLogin")]
        public int LogInUser(MyUser myUser)
        {
            if (myUser == null)
            {
                return 0;
            }
            return ContentProcessor.LogInUser(myUser);
        }

        [HttpGet]
        [Route("Letter")]
        public string GetMyLetters(int userId)
        {
            if (userId == 0)
            {
                return null;
            }

            string json = JsonConvert.SerializeObject(ContentProcessor.GetMyLetters(userId));
           // List<MyMessage> myMessageJSON = JsonConvert.DeserializeObject<List<MyMessage>>(json);
            return json;
        }

        [HttpPost]
        [Route("Letter")]
        public bool SendNewLetter(MyMessage myMessage)
        {
            if (myMessage == null)
            {
                return false;
            }
            return ContentProcessor.SendLetter(myMessage);
        }

        [HttpDelete]
        [Route("Letter")]
        public bool DeleteLetter(int letterId)
        {
            if (letterId == 0)
            {
                return false;
            }
            return Repositories.LoginRepository.DeleteLetterById(letterId);
        }

        /////////////////////////////////////////////////







    }

     
}
