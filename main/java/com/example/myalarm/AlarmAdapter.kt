package com.example.myalarm

import android.app.AlarmManager
import android.app.PendingIntent
import android.content.Context
import android.content.Intent
import android.os.Build
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.CompoundButton
import android.widget.TextView
import android.widget.Toast
import androidx.annotation.RequiresApi
import androidx.core.content.ContextCompat.getSystemService
import androidx.recyclerview.widget.RecyclerView
import kotlinx.android.synthetic.main.item_alarm.view.*
import java.util.*


class AlarmAdapter(val context: Context, val alarms: List<Alarm>, val itemClick: (Alarm)->Unit) :
    RecyclerView.Adapter<AlarmAdapter.Holder>() {
    var currentPosition : Int = 0
    lateinit var  am: AlarmManager
    lateinit var pi : PendingIntent

    override fun onCreateViewHolder(parent: ViewGroup, viewType: Int): Holder {
        val view = LayoutInflater.from(context).inflate(R.layout.item_alarm, parent, false)
        return Holder(view)
    }

    @RequiresApi(Build.VERSION_CODES.KITKAT)
    override fun onBindViewHolder(holder: Holder, position: Int) {
        holder?.bind(alarms[position],position)
    }

    override fun getItemCount(): Int {
        return alarms.size
    }

    inner class Holder(itemView: View?) : RecyclerView.ViewHolder(itemView!!) {
        val nameTv = itemView?.findViewById<TextView>(R.id.itemName)
        val lifeTv = itemView?.findViewById<TextView>(R.id.itemLifeSpan)

        @RequiresApi(Build.VERSION_CODES.KITKAT)
        fun bind(alarm: Alarm, pos:Int) {
            nameTv?.text = alarm.alarmTime
            lifeTv?.text = alarm.alarmNote
            currentPosition = pos

            itemView.setOnClickListener{
                itemClick(alarm)
            }

            itemView.switch1.setOnCheckedChangeListener{_, isChecked: Boolean ->
                if(isChecked)
                {
                    var calendar:Calendar = Calendar.getInstance()
                    am = context.getSystemService(Context.ALARM_SERVICE) as AlarmManager
                    var myIntent: Intent = Intent(context,AlarmReceiver::class.java)

                    var results = alarm.alarmTime?.split(":")
                    var hour = results!![0].toInt()
                    var minute = results!![1].toInt()

                    calendar.set(Calendar.HOUR_OF_DAY, hour)
                    calendar.set(Calendar.MINUTE,minute)
                    calendar.set(Calendar.SECOND,0)
                    calendar.set(Calendar.MILLISECOND,0)
                    myIntent.putExtra("extra" , "on")
                    myIntent.putExtra("AlarmId",(pos+1).toString())
                    pi = PendingIntent.getBroadcast(context,(pos+1),myIntent, PendingIntent.FLAG_UPDATE_CURRENT)
                    am.setExact(AlarmManager.RTC_WAKEUP,calendar.timeInMillis,pi)
                }
                else{
                    var calendar:Calendar = Calendar.getInstance()
                    am = context.getSystemService(Context.ALARM_SERVICE) as AlarmManager
                    var myIntent: Intent = Intent(context,AlarmReceiver::class.java)

                    var results = alarm.alarmTime?.split(":")
                    var hour = results!![0].toInt()
                    var minute = results!![1].toInt()

                    calendar.set(Calendar.HOUR_OF_DAY, hour)
                    calendar.set(Calendar.MINUTE,minute)
                    calendar.set(Calendar.SECOND,0)
                    calendar.set(Calendar.MILLISECOND,0)

                    myIntent.putExtra("extra" , "off")
                    pi = PendingIntent.getBroadcast(context,(pos + 1),myIntent,PendingIntent.FLAG_UPDATE_CURRENT)
                    am.cancel(pi)
                    context.sendBroadcast(myIntent)
                }
            }
        }

    }
}