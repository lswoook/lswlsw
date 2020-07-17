package com.example.myalarm

import androidx.room.ColumnInfo
import androidx.room.Entity
import androidx.room.PrimaryKey

/* Cat.kt */

@Entity(tableName = "alarm")
class Alarm(@PrimaryKey var id: Long?,
            @ColumnInfo(name = "alarmTime") var alarmTime: String?,
            @ColumnInfo(name = "alarmNote") var alarmNote: String
){
    constructor(): this(null,"","")
}