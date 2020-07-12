﻿using System;

namespace Firebase
{
    [Serializable]
    public class User
    {
        public string userId;
        public string nickName;
        public double score;

        public User(string id, string nick, double score)
        {
            userId = id;
            nickName = nick;
            this.score = score;
        }
        
        public User( string nick, double score)
        {
            nickName = nick;
            this.score = score;
        }

        public override string ToString()
        {
            return $"id: {userId}, nick: {nickName}, score: {score}";
        }
    }
}