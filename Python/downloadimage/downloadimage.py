import urllib.request
import os.path
import xlrd
from PIL import Image
import shutil


def getUrlFromExcel(excelFilePath):
    imageUrlList = list()

    wb = xlrd.open_workbook(excelFilePath)
    sheet = wb.sheet_by_index(0)
    
    for i in range(sheet.nrows):
        imageUrl = str(sheet.cell_value(i, 0)).strip()
        imageUrlList.append(imageUrl)
    return imageUrlList

def downloadImage(imageUrl, fileName):
    urllib.request.urlretrieve(imageUrl, fileName)

def resizeImage(srcPath, destPath, height, width):
    image = Image.open(srcPath)
    resized_image = image.resize((height, width))
    resized_image.save(destPath)

def start():
    downloadImageDir ="Download/"
    saveImageDir = "WebImage/"
    excelFilePath = "Nordicproductionwebimage.xlsx"
    
    if not os.path.isdir(saveImageDir):
        # shutil.rmtree(saveImageDir)
        os.mkdir(saveImageDir)
    if os.path.isdir(downloadImageDir):
        shutil.rmtree(downloadImageDir)
    
    os.mkdir(downloadImageDir)
    
    imageUrlList = getUrlFromExcel(excelFilePath)

    for idx, imageUrl in enumerate(imageUrlList):
        try:
            print(str(idx) + " " + imageUrl)
            fileName = os.path.basename(imageUrl)
            downloadImage(imageUrl, os.path.join(downloadImageDir, fileName))
            newFileName = os.path.splitext(fileName)[0] + "_thumb200" + os.path.splitext(fileName)[1]
            resizeImage(os.path.join(downloadImageDir, fileName), os.path.join(saveImageDir, newFileName), 200, 200)
            os.remove(os.path.join(downloadImageDir, fileName)) 
        except Exception as e:
            print(str(idx) + " " + imageUrl + " " + e)


start()