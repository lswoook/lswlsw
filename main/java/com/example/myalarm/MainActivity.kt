package com.example.myalarm

import android.content.Intent
import androidx.appcompat.app.AppCompatActivity
import android.os.Bundle
import android.util.Log
import android.view.View
import android.widget.PopupMenu
import android.widget.Toast
import androidx.recyclerview.widget.LinearLayoutManager
import kotlinx.android.synthetic.main.activity_main.*
import kotlinx.android.synthetic.main.item_alarm.*

class MainActivity : AppCompatActivity() {
    private var alarmList = listOf<Alarm>()
    private var alarmDb : AlarmDB? = null
    var mAdapter = AlarmAdapter(this, alarmList){
    }

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_main)

        //메인화면 시작시 데이터베이스에서 데이터 불러오기
        alarmDb = AlarmDB.getInstance(this)
        val r = Runnable {
            try {
                alarmList = alarmDb?.alarmDao()?.getAll()!!
            } catch (e: Exception) {
                Log.d("tag", "Error - $e")
            }
            this.runOnUiThread {
                mAdapter = AlarmAdapter(this, alarmList){
                        alarm->
                    var i2 = Intent(this,AlarmEdit::class.java)
                    i2.putExtra("DBid",alarm.id.toString())
                    startActivity(i2)
                    finish()
                }
                mRecyclerView.adapter = mAdapter
                mRecyclerView.layoutManager = LinearLayoutManager(this)
                mRecyclerView.setHasFixedSize(true)
                mAdapter.notifyDataSetChanged()
            }
        }
        val thread = Thread(r)
        thread.start()

        //추가 버튼 누름
        mAddBtn.setOnClickListener {
            val i = Intent(this, AlarmSetting::class.java)
            startActivity(i)
            finish()
        }
    }
    override fun onDestroy() {
        AlarmDB.destroyInstance()
        super.onDestroy()
    }
}
