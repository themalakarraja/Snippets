import pandas as pd
import qrcode
import os.path

excelPath = "20211202045613_FPAA.data.format.APM_FSA_Batch_1 - Copy.xlsx"
qrSavePath = "Glenn Story/"
sheetName = "Back of Card "

df = pd.read_excel(excelPath, sheet_name=sheetName)

if not os.path.isdir(qrSavePath):  # Create QR dir if not exists
    os.mkdir(qrSavePath)

for row in range(df.shape[0]):
    qr = qrcode.make(df.loc[row][2])  # Create QR code
    qrName = "QR." + df.loc[row][1].replace("Mr ", "").replace("Ms ", "") + '.png'
    qr = qr.resize((84, 84))  # Resize qr
    qr.save(qrSavePath + qrName)  # Save qr


# for row in range(df.shape[0]):
#     qrName1 = df.loc[row][1].replace("Mr ", "").replace("Ms ", "")
#     for row2 in range(row+ 1, df.shape[0]):
#         qrName2 = df.loc[row2][1].replace("Mr ", "").replace("Ms ", "")
#         if qrName1 == qrName2:
#             print(qrName1)