package com.east2west.octopus;

import android.app.ListActivity;
import android.os.Bundle;
import android.widget.ArrayAdapter;

public class LeaderboardsActivity extends ListActivity {
	@Override
	public void onCreate(Bundle savedInstanceState) {
		super.onCreate(savedInstanceState);
		if (MainActivity.str != null)
			setListAdapter(new ArrayAdapter<String>(this,
					android.R.layout.simple_list_item_1, MainActivity.str));
	}
}
