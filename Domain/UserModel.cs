using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess;

namespace Domain
{
    public class UserModel
    {

        public bool hwidmatchfromdao;
        public bool checkexpdatefromdao;
        public DateTime expdate;

        UserDAO userDao = new UserDAO();/*
        public bool LoginUser(string email,string pwd) {

          
            return userDao.Login(email,pwd);
           
        }*/
        public void LogoutUser()
        {
            userDao.Logout(0);

        }
        public void checkhwidmatch() {

            hwidmatchfromdao = userDao.hwidmatch; 

        }
        public void checkexpdate()
        {
            expdate = userDao.publicexpdate;
            checkexpdatefromdao = userDao.checkedexpdate;

        }

        public bool CheckHWID(string hwid) {

            return userDao.CheckHWID(hwid);
        }
        public string GetMachineGuid() {

            return userDao.GetMachineGuid();
        }


    }

}
