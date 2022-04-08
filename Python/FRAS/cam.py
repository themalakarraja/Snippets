import getpass

#import cv2

# vid = cv2.VideoCapture(0)
# vid = cv2.VideoCapture(getpass.rtsp)

# while(True):
# 	ret, frame = vid.read()
# 	cv2.imshow('frame', frame)
# 	if cv2.waitKey(1) & 0xFF == ord('q'):
# 		break

# vid.release()
# cv2.destroyAllWindows()



import cv2
# cap = cv2.VideoCapture(0)
cap = cv2.VideoCapture(getpass.rtsp)

fourcc = cv2.VideoWriter_fourcc(*'MJPG')
out = cv2.VideoWriter('output.avi', fourcc, 20.0, (640, 480))

while(True):
	ret, frame = cap.read()
	if ret and frame is not None:
		out.write(frame)
		cv2.imshow('Original', frame)
	else:
		print("None")

	# Wait for 'a' key to stop the program
	if cv2.waitKey(1) & 0xFF == ord('a'):
		break

cap.release()
out.release()
cv2.destroyAllWindows()


#import cv2

# cap = cv2.VideoCapture(getpass.rtsp) # it can be rtsp or http stream
# ret, frame = cap.read()

# if cap.isOpened():
#     _,frame = cap.read()
#     cap.release() #releasing camera immediately after capturing picture
#     if _ and frame is not None:
#         cv2.imwrite('latest.jpg', frame)