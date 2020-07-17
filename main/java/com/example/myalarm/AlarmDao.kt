package com.example.myalarm

import androidx.lifecycle.LiveData
import androidx.room.*
import androidx.room.OnConflictStrategy.REPLACE


@Dao
interface AlarmDao {
    @Query("SELECT * FROM alarm")
    fun getAll(): List<Alarm>

    @Query("SELECT * FROM alarm WHERE id = :id")
    fun selectById(id:Int):List<Alarm>

    /* import android.arch.persistence.room.OnConflictStrategy.REPLACE */
    @Insert(onConflict = REPLACE)
    fun insert(cat: Alarm)

    @Query("DELETE from alarm")
    fun deleteAll()

    @Update(onConflict = OnConflictStrategy.ABORT)
    fun update(cat:Alarm)

    @Delete
    fun delete(cat: Alarm)

}