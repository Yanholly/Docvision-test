using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebLogins.Models;
using WebLogins.Repositories;

namespace WebLogins.Processors
{
    public class ContentProcessor
    {
        //Проверка данных юзера. Если есть в базе и активен, получаем его номер. Если нет - 0
        public static int LogInUser(MyUser myUser)
        {
            if (myUser == null || myUser.Login.Length > 10 || myUser.Password.Length > 10 || myUser.Login.Length == 0 || myUser.Password.Length == 0)
                return 0;
            return LoginRepository.CheckUser(myUser);
        }

        //Регистрация нового юзера. Если пользователь с подобным ником уже существует или данные некорректны - 0, иначе айди нового юзера
        public static int RegisterNewUser(MyUser myUser)
        {
            if (myUser == null || myUser.Login.Length > 10 || myUser.Password.Length > 10 || LoginRepository.FindUser(myUser.Login) > 0 || myUser.Login.Length == 0 || myUser.Password.Length == 0)
                return 0;

            LoginRepository.AddUserToDB(myUser);

            return LoginRepository.FindUser(myUser.Login);
        }

        //Отправка письма. True если письмо добавлено в базу. False если оно не соответствует требованиям
        public static bool SendLetter(MyMessage myMessage)
        {
            bool tooBig = false;
            for (int i = 0; i < myMessage.Tags.Count(); i++)
                if (myMessage.Tags[i].Length > 10)
                {
                    tooBig = true;
                    break;
                }
            if (myMessage == null || myMessage.Address.Length > 10 || myMessage.Sender.Length > 10 || myMessage.Title.Length > 50 || tooBig
                                  || LoginRepository.FindUser(myMessage.Sender) < 1 || LoginRepository.FindUser(myMessage.Address) < 1)
                return false;

            int letterId = LoginRepository.AddMessageToDB(myMessage);

            if (myMessage.Tags != null)
                for (int i = 0; i < myMessage.Tags.Count(); i++)
                {
                    if (LoginRepository.CheckTag(myMessage.Tags[i]) == 0)
                        LoginRepository.AddTagToDB(myMessage.Tags[i]);
                    LoginRepository.AddTagLetter(LoginRepository.CheckTag(myMessage.Tags[i]), letterId);
                }
            return true;
        }

        //Получение всех писем пользователя. Null если пользователя не существует. 
        public static List<MyMessage> GetMyLetters(int userId)
        {
           if (LoginRepository.GetUserById(userId) == null)
                return null;
           return LoginRepository.GetMessagesFromDB(userId);
        }

    }
}