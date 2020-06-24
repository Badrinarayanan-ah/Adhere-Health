import xlsxwriter
import pandas as pd
import pandas.io.sql
import pyodbc
import sys


# Parameters
server = 'SQLOPS'
db = 'CallCampaigns'
dbTable = sys.argv[1]
filePath = sys.argv[2]

print(dbTable)
print (filePath)


# Create the connection
conn = pyodbc.connect('DRIVER={SQL Server};SERVER=sqlops;DATABASE=callCampaigns;Trusted_Connection=yes')

# query db
sql = "select  * FROM [CCUSDN\CCUHOST].[SDN_DialerStore].[dbo].[" + dbTable + "]"

df = pd.read_sql(sql, conn)
df.head()


workbook = xlsxwriter.Workbook(filePath,{'nan_inf_to_errors': True} )
worksheet = workbook.add_worksheet()

header_format = workbook.add_format({'bold':False, 'font_color': 'white', 'bg_color':'black', 'align':'left'})

worksheet.write(0,0,"RowId", header_format)
worksheet.write(0,1,"Pat_Id", header_format)
worksheet.write(0,2,"Client Name", header_format)
worksheet.write(0,3,"First Name", header_format)
worksheet.write(0,4,"Last Name", header_format)
worksheet.write(0,5,"Mtm Url", header_format)
worksheet.write(0,6,"Phone Number", header_format)
worksheet.write(0,7,"Switch Result", header_format)
worksheet.write(0,8,"Agent Result", header_format)
worksheet.write(0,9,"Result Ts", header_format)
worksheet.write(0,10,"Retry Count", header_format)
worksheet.write(0,11,"Retry Username", header_format)
worksheet.write(0,12,"Retry Ts", header_format)
worksheet.write(0,13,"Retry Index", header_format)
worksheet.write(0,14,"Retry Number", header_format)
worksheet.write(0,15,"Complete Ts", header_format)
worksheet.write(0,16,"State Code", header_format)
worksheet.write(0,17,"Time Zone", header_format)
worksheet.write(0,18,"Dtp Scores", header_format)
worksheet.write(0,19,"Lowbound Ts", header_format)
worksheet.write(0,20,"Highbound Ts", header_format)
worksheet.write(0,21,"Language", header_format)
worksheet.write(0,22,"Zip Code", header_format)
worksheet.write(0,23,"Retry Count", header_format)
worksheet.write(0,24,"Phone Type", header_format)
worksheet.write(0,25,"Contract Id", header_format)
worksheet.write(0,26,"Call List Session Id", header_format)
worksheet.write(0,27,"Call Attempts", header_format)
worksheet.write(0,28,"Created Date", header_format)
worksheet.write(0,29,"Updated Date", header_format)
worksheet.write(0,30,"Created By", header_format)
worksheet.write(0,31,"Updated By", header_format)

worksheet.set_column('A:A', 5.09)
worksheet.set_column('B:B', 9)
worksheet.set_column('C:C', 10.27)
worksheet.set_column('D:D', 9.09)
worksheet.set_column('E:E', 15.95)
worksheet.set_column('F:F', 42.91)
worksheet.set_column('G:G', 12.86)
worksheet.set_column('H:H', 11.41)
worksheet.set_column('I:I', 10.86)
worksheet.set_column('J:J', 7.91)
worksheet.set_column('K:K', 11.23)
worksheet.set_column('L:L', 13.73)
worksheet.set_column('M:M', 7.23)
worksheet.set_column('N:N', 9.82)
worksheet.set_column('O:O', 12.05)
worksheet.set_column('P:P', 10.73)
worksheet.set_column('Q:Q', 9.18)
worksheet.set_column('R:R', 8.59)
worksheet.set_column('S:S', 8.91)
worksheet.set_column('T:T', 16.59)
worksheet.set_column('U:U', 11.59)
worksheet.set_column('V:V', 7.36)
worksheet.set_column('W:W', 7.41)
worksheet.set_column('X:X', 10.23)
worksheet.set_column('Y:Y', 10.73)
worksheet.set_column('Z:Z', 8.73)
worksheet.set_column('AA:AA', 13.45)
worksheet.set_column('AB:AB', 10.32)
worksheet.set_column('AC:AC', 14.55)
worksheet.set_column('AD:AD', 10.91)
worksheet.set_column('AE:AE', 21.27)
worksheet.set_column('AF:AF', 9.05)


number_format = workbook.add_format({'bold':False, 'font_color':'black', 'align':'center'})
number_format.set_num_format_index(0x01)
number_format.set_bottom_color ('#BFBFBF')
number_format.set_top_color ('#BFBFBF')
number_format.set_bottom(3)
number_format.set_top(3)


decimal_format = workbook.add_format({'bold':False, 'font_color': 'black', 'align':'center'})
decimal_format.set_num_format('0.000')
decimal_format.set_bottom_color ('#BFBFBF')
decimal_format.set_top_color ('#BFBFBF')
decimal_format.set_bottom(3)
decimal_format.set_top(3)

text_format = workbook.add_format({'bold':False, 'font_color': 'black', 'align':'left'})
text_format.set_bottom_color ('#BFBFBF')
text_format.set_top_color ('#BFBFBF')
text_format.set_bottom(3)
text_format.set_top(3)


datetime_format = workbook.add_format({'bold':False, 'font_color': 'black', 'align':'left'})
datetime_format.set_num_format_index(0x16)
datetime_format.set_bottom_color ('#BFBFBF')
datetime_format.set_top_color ('#BFBFBF')
datetime_format.set_bottom(3)
datetime_format.set_top(3)

date_format = workbook.add_format({'bold':False, 'font_color': 'black', 'align':'left'})
date_format.set_num_format_index(0x0e)
date_format.set_bottom_color ('#BFBFBF')
date_format.set_top_color ('#BFBFBF')
date_format.set_bottom(3)
date_format.set_top(3)

white_bg = workbook.add_format()
white_bg.set_pattern(1)
white_bg.set_bg_color('#FFFFFF')

gray_bg = workbook.add_format()
gray_bg.set_pattern(1)
gray_bg.set_bg_color('#F2F2F2')

rowNum = 1
col = 0

for (idx, row) in df.iterrows():
    worksheet.write(rowNum,  0, row.loc['rowid'], number_format)
    worksheet.write(rowNum,  1, row.loc['pat_id'], number_format)
    worksheet.write(rowNum,  2, row.loc['Client_name'].strip(), text_format)
    worksheet.write(rowNum,  3, row.loc['First_name'].strip(), text_format)
    worksheet.write(rowNum,  4, row.loc['Last_name'].strip(), text_format)
    worksheet.write(rowNum,  5, row.loc['MTM_Url'].strip(), text_format)
    worksheet.write(rowNum,  6, row.loc['Phone_Number'].strip(), text_format)
    worksheet.write(rowNum,  7, "NULL", text_format)
    worksheet.write(rowNum,  8, "NULL", text_format)
    worksheet.write(rowNum,  9, "NULL", text_format)
    worksheet.write(rowNum,  10, "NULL", text_format)
    worksheet.write(rowNum,  11, "NULL", text_format)
    worksheet.write(rowNum,  12, "NULL", text_format)
    worksheet.write(rowNum,  13, "NULL", text_format)
    worksheet.write(rowNum,  14, "NULL", text_format)
    worksheet.write(rowNum,  15, "NULL", text_format)
    worksheet.write(rowNum,  16, (row.loc['State_code'] or 'NULL').strip(), text_format)
    worksheet.write(rowNum,  17, ( row.loc['time_zone'] or 'NULL').strip(), text_format)
    worksheet.write(rowNum,  18, (row.loc['dtp_scores'] or 0), number_format)
    worksheet.write(rowNum,  19, (row.loc['lowbound_TS'] + 0), decimal_format)
    worksheet.write(rowNum,  20, (row.loc['highbound_TS'] + 0), decimal_format)
    worksheet.write(rowNum,  21, (row.loc['language'] or 'NULL').strip(), text_format)
    worksheet.write(rowNum,  22, (row.loc['Zip_code'] or 'NULL').strip(), text_format)
    worksheet.write(rowNum,  23, "NULL", text_format)
    worksheet.write(rowNum,  24, (row.loc['PHONE_TYPE'] or 'NULL').strip(), text_format)
    worksheet.write(rowNum,  25, (row.loc['ContractID'] or 'NULL').strip(), text_format)
    worksheet.write(rowNum,  26, row.loc['CallListSessionID'], number_format)
    worksheet.write(rowNum,  27, row.loc['callAttempts'], number_format)
    worksheet.write(rowNum,  28, row.loc['CreatedDate'], datetime_format)
    worksheet.write(rowNum,  29, row.loc['UpdatedDate'], datetime_format)
    worksheet.write(rowNum,  30, (row.loc['CreatedBy'] or 'NULL').strip(), text_format)
    worksheet.write(rowNum,  31, (row.loc['UpdatedBy'] or 'NULL').strip(), text_format)
    rowNum += 1

worksheet.conditional_format(1,0,rowNum,31, {'type':     'formula',
                                       'criteria': '=(ISODD(ROW()))',
                                       'format':   gray_bg})
worksheet.conditional_format(1,0,rowNum,31, {'type':     'formula',
                                       'criteria': '=(ISEVEN(ROW()))',
                                       'format':   white_bg})


workbook.close()