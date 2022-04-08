import os
from PIL import Image

def resizeImage(srcPath, destPath, height, width):
    image = Image.open(srcPath)
    resized_image = image.resize((height, width))
    resized_image.save(destPath)


srcFolderPath = r"C:\_raja\New folder\ProductImages"
destFolderPath = r"C:\_raja\New folder\Thumbnails"
resizeX = 300
resizeY = 300
appendText = "_thumb300"

image_list = os.listdir(srcFolderPath)

for idx, image in enumerate(image_list):
    print(idx)
    fileName = os.path.splitext(image)[0] + appendText + os.path.splitext(image)[1]
    srcFilePath = os.path.join(srcFolderPath, image)
    destFilePath = os.path.join(destFolderPath, fileName)
    resizeImage(srcFilePath, destFilePath, resizeX, resizeY)
    
    