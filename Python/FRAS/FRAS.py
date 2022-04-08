import cv2
import numpy as np
import face_recognition

# Conver BGR to RGB
imgElon = face_recognition.load_image_file('Training_images/pm1.jpg')
imgElon = cv2.cvtColor(imgElon, cv2.COLOR_BGR2RGB)

imgTest = face_recognition.load_image_file('pm2.jpg')
imgTest = cv2.cvtColor(imgTest, cv2.COLOR_BGR2RGB)

# Finding face location
faceLoc = face_recognition.face_locations(imgElon)[0]
encodeElon = face_recognition.face_encodings(imgElon)[0]
cv2.rectangle(imgElon, (faceLoc[3], faceLoc[0]), (faceLoc[1], faceLoc[2]), (0, 255, 0), 2)

faceLocTest = face_recognition.face_locations(imgTest)[0]
encodeTest = face_recognition.face_encodings(imgTest)[0]
cv2.rectangle(imgTest, (faceLocTest[3], faceLocTest[0]), (faceLocTest[1], faceLocTest[2]), (0, 255, 0), 1)

# Comparing faces and finding distances b/w them (128 mesurements for both the images) linear svm to find out wherather they match or not
results = face_recognition.compare_faces([encodeElon], encodeTest)
faceDistance = face_recognition.face_distance([encodeElon], encodeTest)
print(results)
print(faceDistance)

# Show image
cv2.imshow("imgElon", imgElon)
cv2.imshow("imgTest", imgTest)

cv2.waitKey(0)