package com.example.myalarm

import android.content.Context
import androidx.room.Database
import androidx.room.Room
import androidx.room.RoomDatabase


@Database(entities = [Alarm::class], version = 1)
abstract class AlarmDB: RoomDatabase() {
    abstract fun alarmDao(): AlarmDao

    companion object {
        private var INSTANCE: AlarmDB? = null

        fun getInstance(context: Context): AlarmDB? {
            if (INSTANCE == null) {
                synchronized(AlarmDB::class) {
                    INSTANCE = Room.databaseBuilder(context.applicationContext,
                        AlarmDB::class.java, "alarm.db")
                        .fallbackToDestructiveMigration()
                        .build()
                }
            }
            return INSTANCE
        }

        fun destroyInstance() {
            INSTANCE = null
        }
    }
}