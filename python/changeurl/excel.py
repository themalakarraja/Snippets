import os.path
import xlrd
import shutil
from PIL import Image

excelPath = r"C:\_raja\changeurl\Style With Color Code.xlsx"
destPath = r"C:\_raja\changeurl\images"

imgPath = r"C:\_raja\changeurl\501-AUB.jpg"

def copyFileToLocal(srcFilePath, destPath, newFileName):
    try:
        newFileName = newFileName + os.path.splitext(srcFilePath)[1]
        destFilePath = os.path.join(destPath, newFileName)
        if not os.path.exists(srcFilePath):
            srcFilePath = os.path.join(os.path.split(srcFilePath)[0], newFileName)
        try:
            shutil.copyfile(srcFilePath, destFilePath)
        except Exception as e:
            print(e)
    except Exception as e:
        print("Error " + e)

def readExcel():
    wb = xlrd.open_workbook(excelPath)
    sheet = wb.sheet_by_index(0)

    for i in range(1, sheet.nrows):
        path = str(sheet.cell_value(i, 3)).strip()
        fileName = str(sheet.cell_value(i, 0)).strip()
        if path == "" or path.lower() == "missing":
            continue
        
        copyFileToLocal(path, destPath, fileName)
        # print(fileName + " " + path)


def resizeImage(srcPath, destPathImage, height, width):
    image = Image.open(srcPath)
    resized_image = image.resize((height,width))
    resized_image.save(os.path.join(destPathImage, os.path.basename(srcPath)))

r200 = r"C:\_raja\changeurl\200x200"
r500 = r"C:\_raja\changeurl\500x500"

fileArr = os.listdir(destPath)
for f in fileArr:
    resizeImage(os.path.join(destPath, f), r500, 500, 500)
    